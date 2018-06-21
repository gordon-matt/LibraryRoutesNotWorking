import $ from 'jquery';
import { HttpClient } from 'aurelia-http-client';

import { GenericHttpInterceptor } from '/aurelia-app/embedded/Framework.Web.CommonResources.Scripts.generic-http-interceptor';

export class ViewModel {
    apiUrl = "/odata/framework/web/LocalizableStringApi";

    constructor() {
        const self = this;

        this.datasource = {
            type: 'odata-v4',
            transport: {
                read: {
                    url: "", // Because we need a param, we have to set this in activate() and we ensure autoBind is set to false on the grid (see the view markup)
                    dataType: "json"
                },
                update: {
                    url: `${this.apiUrl}/Default.PutComparitive`,
                    dataType: "json",
                    contentType: "application/json",
                    type: "POST"
                },
                destroy: {
                    url: `${this.apiUrl}/Default.DeleteComparitive`,
                    dataType: "json",
                    contentType: "application/json",
                    type: "POST"
                },
                parameterMap: function (options, operation) {
                    if (operation === "read") {
                        // This is bugged - it returns the old odata format instead of odata-v4
                        //return kendo.data.transports['odata-v4'].parameterMap(options);

                        // So we need to manually fix it:
                        var paramMap = kendo.data.transports.odata.parameterMap(options);
                        if (paramMap.$inlinecount) {
                            if (paramMap.$inlinecount == "allpages") {
                                paramMap.$count = true;
                            }
                            delete paramMap.$inlinecount;
                        }
                        if (paramMap.$filter) {
                            paramMap.$filter = paramMap.$filter.replace(/substringof\((.+),(.*?)\)/, "contains($2,$1)");
                        }
                        return paramMap;
                    }
                    else if (operation === "update") {
                        return kendo.stringify({
                            cultureCode: self.cultureCode,
                            key: options.Key,
                            entity: options
                        });
                    }
                    else if (operation === "destroy") {
                        return kendo.stringify({
                            cultureCode: self.cultureCode,
                            key: options.Key
                        });
                    }
                }
            },
            schema: {
                model: {
                    id: "Key",
                    fields: {
                        Key: { type: "string", editable: false },
                        InvariantValue: { type: "string", editable: false },
                        LocalizedValue: { type: "string" }
                    }
                }
            },
            batch: false,
            pageSize: 10,
            serverPaging: true,
            serverFiltering: true,
            serverSorting: true,
            sort: { field: "Key", dir: "asc" }
        };

        this.http = new HttpClient();
        this.http.configure(config => {
            config.withInterceptor(new GenericHttpInterceptor());
        });
    }

    // Aurelia Component Lifecycle Methods

    activate(params, routeConfig) {
        this.cultureCode = params.cultureCode;

        if (!this.cultureCode) { // could be undefined
            this.cultureCode = null;
        }

        this.datasource.transport.read.url = `${this.apiUrl}/Default.GetComparitiveTable(cultureCode='${this.cultureCode}')`;
    }

    async attached() {
        // Load translations first, else will have errors
        let response = await this.http.get("/admin/localization/localizable-strings/get-translations");
        this.translations = response.content;

        this.datasource.pageSize = $("#GridPageSize").val();
    }

    // END: Aurelia Component Lifecycle Methods

    exportFile() {
        var downloadForm = $("<form>")
            .attr("method", "GET")
            .attr("action", "/admin/localization/localizable-strings/export/" + this.cultureCode);
        $("body").append(downloadForm);
        downloadForm.submit();
        downloadForm.remove();
    }
    
    refreshGrid() {
        this.grid.dataSource.read();
        this.grid.refresh();
    }
}