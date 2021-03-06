This code is taken from a larger project. It's here to demonstrate a problem with library routes in .NET Core 2.1. What's happening is that HTTP 404s are given for routes to controllers in libraries... this only happens when the app is deployed and not when running inside Visual Studio... and this was NOT a problem in .NET Core 2.0. It's only started happening since the update to 2.1.

## Instructions

To run the app, do the following:

1. Install [NodeJS](https://nodejs.org/en/download/)
2. Install JSPM globally: `npm install -g jspm`
3. Clone/download this project
4. Restore JSPM packages: `jspm install`
> **NOTE:** Do this from the root directory of the "MainApp" project (not the solution root)
5. Restore NPM packages: `npm install`
> **NOTE:** Do this from the root directory of the "MainApp" project (not the solution root)
6. Launch the Visual Studio solution and open the **gulpfile.js** in **Task Runner Explorer**
7. Run the **bundle** task
8. Run **MainApp** and browse to **/admin**

Once you're in the admin area, click the Tenants menu. You should notice everything is fine in Visual Studio, but if you now deploy this solution to IIS, you will get 404 messages in the Network tab of the browser's developer tools.