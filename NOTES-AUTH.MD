# Notes on adding authentication

See also Github sample here: https://github.com/Azure-Samples/active-directory-aspnetcore-webapp-openidconnect-v2

## Using Open ID Connect with Azure AD

- Need the Nuget package:

~~~sh
dotnet add package Microsoft.AspNetCore.Authentication.AzureAD.UI --version 2.1.1
~~~

- Need to adapt `Startup.cs` to include:

~~~cs
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

// ...

public void ConfigureServices(IServiceCollection services)
{
    // ...
    // configure Open ID Connect authentication for AAD:
    services.AddAuthentication()
            .AddAzureAD(opts => {
                Configuration.Bind("AzureAd", opts);
            });

    // choose not to validate which directory issued the id_token, so this works with any AAD tenant:
    // note that you need to configure the right scheme name for these options, or they wont be seen by the authentication handler:
            services.Configure<OpenIdConnectOptions>(AzureADDefaults.OpenIdScheme, opts => {
                opts.Authority = opts.Authority + "/v2.0/";
                opts.TokenValidationParameters.ValidateIssuer = false;
            });
    // ...
}

public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
    // ...
    // configure app to use authentication, injecting the middleware in the pipeline:
            app.UseAuthentication();
    // ...
}

~~~

- Add a secured controller and matching view:

~~~cs
using Microsoft.AspNetCore.Authorization;

[Authorize]
public class SecuredController : Controller
{
  
    public SecuredController(){}
    public IActionResult Index()
    {
        return View();
    }
}
~~~

~~~cshtml
<div class="jumbotron" 
  <h1 class="display-4">
      This is a secure authenticated application area
  </h1>
</div>
~~~

- Add the AAD authentication configuration within `appsettings.json`

~~~json
{
  "AzureAd": {
    "Instance": "https://login.microsoftonline.com/",
    "Domain": "[Enter the domain of your tenant, e.g. contoso.onmicrosoft.com]",
    "TenantId": "[Enter 'common', or 'organizations' or the Tenant Id (Obtained from the Azure portal. Select 'Endpoints' from the 'App registrations' blade and use the GUID in any of the URLs), e.g. da41245a5-11b3-996c-00a8-4d99re19f292]",
    "ClientId": "[Enter the Client Id (Application ID obtained from the Azure portal), e.g. ba74781c2-53c2-442a-97c2-3d60re42f403]",
    "CallbackPath": "/signin-oidc"
  }
}
~~~

- register the application with aad:

~~~sh
# first login to the correct tenant in which to register the app:
az login -t myaadtesttenant.onmicrosoft.com --allow-no-subscriptions

# register the app:
az ad app create --display-name netcoreminiapp --homepage https://mvctestappwindows.azurewebsites.net/ --identifier-uris "http://mvctestappwindows" --reply-urls "https://mvctestappwindows.azurewebsites.net/signin-oidc"
~~~

- then capture the `appId` from the json output, use it as the `ClientId` within `appsettings.json`
- verify that in the azure portal, there's an application entry under `App Registrations` within the AAD blade, but none in the `Enterprise Applications` blade.
- without first creating a service principal, the user authentication will fail, saying "access_denied", "misconfigured application..."
- create a service principal by clicking in the portal: "Create Service Principal", which will register this application in the local directory
- by default, no users are assigned to the application
- one also NEEDS to have at least some permissions required for the app; in the case of OIDC: the Microsoft Graph "openid" delegated permission

