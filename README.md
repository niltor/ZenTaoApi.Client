# ZenTaoApi.Client

一个面向 .NET 8+ 的禅道 RESTful API v1 强类型客户端。项目按资源分类封装请求，并将请求 DTO、响应 DTO 与 HTTP 传输层分离。

## 特性

- 初始化时必须提供禅道地址、用户名和密码，并自动调用 `POST /tokens` 获取 Token。
- 后续请求自动携带 `Token` 请求头；遇到一次 `401 Unauthorized` 时自动重新获取 Token 并重试一次。
- 覆盖官方 V1 文档中的部门、用户、项目集、产品、产品计划、发布、需求、项目、版本、执行、任务、Bug、用例、测试单、反馈和工单接口。
- 所有资源 Client 使用强类型 Request DTO 和 Response DTO；不同禅道版本新增的字段会保存在 `AdditionalProperties` 中。
- 使用 `HttpClient`，支持注入已有实例、取消请求和分页/过滤查询参数。
- 支持 ASP.NET Core：固定账号可注入共享客户端，动态账号可通过工厂按需创建并释放客户端。
- 不把账号密码写入日志或源代码；推荐从配置系统或环境变量读取。

## 安装

```bash
dotnet add package ZenTaoApi.Client
```

## 快速开始

`CreateAsync` 完成认证后才会返回客户端，因此客户端返回时已经可以直接调用业务接口：

```csharp
using ZenTaoApi.Client;
using ZenTaoApi.Client.Models.RequestDtos;

await using var client = await ZenTaoClient.CreateAsync(
    baseUrl: "https://zentao.example.com/api.php/v1",
    account: Environment.GetEnvironmentVariable("ZENTAO_ACCOUNT")!,
    password: Environment.GetEnvironmentVariable("ZENTAO_PASSWORD")!);

var products = await client.Products.GetListAsync(new ProductListRequest
{
    Page = 1,
    Limit = 20
});

foreach (var product in products.Products)
{
    Console.WriteLine($"{product.Id}: {product.Name}");
}

var currentUser = await client.Users.GetCurrentAsync();
Console.WriteLine(currentUser.Profile?.RealName);
```

请求和响应模型位于以下目录：

- `Models/RequestDtos`：Token、列表查询、创建、修改和状态操作请求。
- `Models/ResponseDtos`：列表、详情、用户资料、实体和操作响应。
- `Clients`：每个资源一个独立的 Client 类。

例如创建任务时不再传入匿名 `object`：

```csharp
using ZenTaoApi.Client.Models.RequestDtos;

var task = await client.Tasks.CreateAsync(
    executionId: 10,
    new CreateTaskRequest
    {
        Name = "编写接口测试",
        Type = "devel",
        Priority = 3,
        AssignedTo = "xuesong.chen",
        Estimate = 4
    });
```

## 资源分类

| 客户端属性 | 覆盖范围 |
| --- | --- |
| `Departments` | 部门列表、详情 |
| `Users` | 当前用户、用户列表、详情、创建、修改、删除 |
| `ProjectSets` | 项目集的增删改查 |
| `Products` | 产品的增删改查 |
| `ProductPlans` | 产品计划及关联/取消关联需求、Bug |
| `Releases` | 产品发布、项目发布列表 |
| `Requirements` | 产品/项目/执行需求列表、需求增删改、变更、关闭 |
| `Projects` | 项目的增删改查 |
| `Versions` | 项目/执行版本列表及版本增删改查 |
| `Executions` | 执行的增删改查 |
| `Tasks` | 任务增删改查、开始、暂停、继续、完成、关闭、工时 |
| `Bugs` | Bug 增删改查、确认、关闭、激活、解决 |
| `TestCases` | 用例增删改查、执行 |
| `TestTasks` | 测试单列表、详情 |
| `Feedbacks` | 反馈增删改查、指派、关闭 |
| `Tickets` | 工单增删改查 |

分页和筛选参数使用资源对应的强类型 Request DTO，例如：

```csharp
var tasks = await client.Tasks.GetForExecutionAsync(
    executionId: 10,
    request: new TaskListRequest
    {
        Status = "wait",
        Page = 1,
        Limit = 50
    });
```

对于尚未封装的新 V1 接口，可以通过泛型通用入口继续保持强类型：

```csharp
using ZenTaoApi.Client.Models.ResponseDtos;

var result = await client.RequestAsync<ProductListResponse>(
    HttpMethod.Get,
    "products",
    query: new ProductListRequest { Page = 1 });
```

## HttpClient 注入

如果应用使用 `IHttpClientFactory` 或自定义 `HttpMessageHandler`，可以传入已有的 `HttpClient`。客户端不会释放外部传入的实例：

```csharp
using var httpClient = new HttpClient
{
    Timeout = TimeSpan.FromSeconds(30)
};

await using var client = await ZenTaoClient.CreateAsync(
    "https://zentao.example.com/api.php/v1",
    account,
    password,
    httpClient);
```

## ASP.NET Core 依赖注入

固定服务账号通过 `AddZenTaoClient` 注册为应用级共享实例。配置中的密码建议使用 User Secrets、环境变量或密钥管理服务，不要提交到 `appsettings.json`：

```csharp
using ZenTaoApi.Client;
using ZenTaoApi.Client.DependencyInjection;

builder.Services.AddZenTaoClient(options =>
{
    options.BaseUrl = builder.Configuration["ZenTao:BaseUrl"] ?? "";
    options.Account = builder.Configuration["ZenTao:Account"] ?? "";
    options.Password = builder.Configuration["ZenTao:Password"] ?? "";
});
```

注入的 `ZenTaoClient` 在应用内共享，Token 获取和失效后的刷新由客户端管理；首次业务请求时异步获取 Token。若希望应用启动时就验证连通性和账号，可在 `builder.Build()` 后、`app.RunAsync()` 前调用 `AuthenticateAsync()`。

```csharp
using ZenTaoApi.Client.Models.RequestDtos;
using ZenTaoApi.Client.Models.ResponseDtos;

public sealed class ProductService(ZenTaoClient client)
{
    public Task<ProductListResponse> GetProductsAsync(CancellationToken cancellationToken) =>
        client.Products.GetListAsync(new ProductListRequest { Page = 1, Limit = 20 }, cancellationToken);
}
```

账号在运行时变化时，通过 `ZenTaoClientFactory` 创建独立、已认证的客户端；使用方负责释放。若应用只需要动态账号而不需要固定账号，可仅调用 `AddZenTaoClientFactory()` 注册工厂：

```csharp
using ZenTaoApi.Client;
using ZenTaoApi.Client.Models.RequestDtos;
using ZenTaoApi.Client.Models.ResponseDtos;

builder.Services.AddZenTaoClientFactory();

public sealed class TenantProductService(ZenTaoClientFactory clientFactory)
{
    public async Task<ProductListResponse> GetProductsAsync(
        string baseUrl,
        string account,
        string password,
        CancellationToken cancellationToken)
    {
        await using var client = await clientFactory.CreateAsync(
            baseUrl, account, password, cancellationToken);

        return await client.Products.GetListAsync(
            new ProductListRequest { Page = 1, Limit = 20 }, cancellationToken);
    }
}
```

同时需要固定账号和动态账号时，只需调用 `AddZenTaoClient(...)`：它会注册共享客户端及动态客户端工厂。动态客户端使用完毕后应立即释放，避免账号对应的 Token 和 HTTP 资源被长期保留。

## 本地测试

```bash
dotnet restore ZenTaoApi.Client.slnx
dotnet test ZenTaoApi.Client.slnx --configuration Release
dotnet pack src/ZenTaoApi.Client/ZenTaoApi.Client.csproj --configuration Release
```

如需使用真实禅道实例做手工验证，请通过环境变量传入凭据：

```powershell
$env:ZENTAO_BASE_URL = "http://your-zentao-host/api.php/v1"
$env:ZENTAO_ACCOUNT = "your-account"
$env:ZENTAO_PASSWORD = "your-password"
```

## GitHub Actions 自动发包

`.github/workflows/pack.yml` 会在提交到 `main` 分支时构建、测试、打包并发布 NuGet。工作流使用 NuGet Trusted Publishing 的 OIDC 短期凭据，不需要保存长期 API Key：

1. 在 nuget.org 为仓库创建 Trusted Publishing policy。
2. 将 NuGet 用户名保存为 GitHub Actions secret `NUGET_USER`（使用 NuGet profile name，不是邮箱）。
3. policy 的 workflow file 填写 `pack.yml`。

NuGet Trusted Publishing 还要求工作流拥有 `id-token: write` 权限；该权限已在 `pack.yml` 中配置。本次发布版本为 `0.1.1`，包版本统一由 `ZenTaoApi.Client.csproj` 中的 `<Version>` 决定；每次发布新包前更新该版本号。

## 文档

接口路径和 Token 约定参考禅道官方 [RESTful API v1 文档](https://www.zentao.net/book/api/664.html)。

## 许可证

MIT
