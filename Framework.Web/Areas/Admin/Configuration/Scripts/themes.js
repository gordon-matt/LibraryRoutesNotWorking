import 'jquery';
import 'bootstrap-notify';
import { HttpClient } from 'aurelia-http-client';

import { GenericHttpInterceptor } from '/aurelia-app/embedded/Framework.Web.CommonResources.Scripts.generic-http-interceptor';

export class ViewModel {
    apiUrl = "/odata/framework/web/ThemeApi";

    constructor() {
        this.datasource = {
            type: 'odata-v4',
            transport: {
                read: this.apiUrl
            },
            schema: {
                model: {
                    fields: {
                        PreviewImageUrl: { type: "string" },
                        Title: { type: "string" },
                        PreviewText: { type: "string" },
                        SupportRtl: { type: "boolean" },
                        MobileTheme: { type: "boolean" },
                        IsDefaultTheme: { type: "boolean" }
                    }
                }
            },
            pageSize: 10,
            serverPaging: true,
            serverFiltering: true,
            serverSorting: true,
            sort: { field: "Title", dir: "asc" }
        };

        this.http = new HttpClient();
        this.http.configure(config => {
            config.withInterceptor(new GenericHttpInterceptor());
        });
    }

    // Aurelia Component Lifecycle Methods

    async attached() {
        // Load translations first, else will have errors
        let response = await this.http.get("/admin/configuration/themes/get-translations");
        this.translations = response.content;

        this.datasource.pageSize = $("#GridPageSize").val();
    }

    // END: Aurelia Component Lifecycle Methods
    
    async setTheme(name) {
        let response = await this.http.post(`${this.apiUrl}/Default.SetTheme`, { themeName: name });

        if (response.isSuccess) {
            $.notify({ message: this.translations.setThemeSuccess, icon: 'fa fa-check' }, { type: 'success' });
        }
        else {
            $.notify({ message: this.translations.setThemeError, icon: 'fa fa-exclamation-triangle' }, { type: 'danger' });
        }

        this.refreshGrid();
    }

    refreshGrid() {
        this.grid.dataSource.read();
        this.grid.refresh();
    }
}