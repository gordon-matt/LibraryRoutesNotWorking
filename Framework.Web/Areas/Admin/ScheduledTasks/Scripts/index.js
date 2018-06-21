import 'jquery';
import 'jquery-validation';
import 'bootstrap-notify';
import { HttpClient } from 'aurelia-http-client';

import { GenericHttpInterceptor } from '/aurelia-app/embedded/Framework.Web.CommonResources.Scripts.generic-http-interceptor';
import { SectionSwitcher } from '/aurelia-app/embedded/Framework.Web.CommonResources.Scripts.section-switching';

export class ViewModel {
    apiUrl = "/odata/framework/web/ScheduledTaskApi";

    constructor() {
        this.datasource = {
            type: 'odata-v4',
            transport: {
                read: this.apiUrl
            },
            schema: {
                model: {
                    id: "Id",
                    fields: {
                        Name: { type: "string" },
                        Seconds: { type: "number" },
                        Enabled: { type: "boolean" },
                        StopOnError: { type: "boolean" },
                        LastStartUtc: { type: "date" },
                        LastEndUtc: { type: "date" },
                        LastSuccessUtc: { type: "date" },
                        Id: { type: "number" }
                    }
                }
            },
            batch: false,
            pageSize: 10,
            serverPaging: true,
            serverFiltering: true,
            serverSorting: true,
            sort: { field: "Name", dir: "asc" }
        };

        this.http = new HttpClient();
        this.http.configure(config => {
            config.withInterceptor(new GenericHttpInterceptor());
        });
    }

    // Aurelia Component Lifecycle Methods

    async attached() {
        // Load translations first, else will have errors
        let response = await this.http.get("/admin/scheduled-tasks/get-translations");
        this.translations = response.content;

        this.datasource.pageSize = $("#GridPageSize").val();

        this.sectionSwitcher = new SectionSwitcher('grid-section');

        this.validator = $("#form-section-form").validate({
            rules: {
                Seconds: { required: true }
            }
        });
    }

    // END: Aurelia Component Lifecycle Methods
    
    async edit(id) {
        let response = await this.http.get(`${this.apiUrl}(${id})`);
        let entity = response.content;

        this.id = entity.Id;
        this.name = entity.Name;
        this.seconds = entity.Seconds;
        this.enabled = entity.Enabled;
        this.stopOnError = entity.StopOnError;

        this.validator.resetForm();
        $("#form-section-legend").html(this.translations.edit);
        this.sectionSwitcher.swap('form-section');
    }

    async runNow(id) {
        let response = await this.http.post(`${this.apiUrl}/Default.RunNow`, { taskId: id });

        if (response.isSuccess) {
            $.notify({ message: this.translations.executedTaskSuccess, icon: 'fa fa-check' }, { type: 'success' });
        }
        else {
            $.notify({ message: this.translations.executedTaskError, icon: 'fa fa-exclamation-triangle' }, { type: 'danger' });
        }

        this.refreshGrid();
    }

    async save() {
        if (!$("#form-section-form").valid()) {
            return false;
        }
        
        let record = {
            Id: this.id,
            Seconds: this.seconds,
            Enabled: this.enabled,
            StopOnError: this.stopOnError
        };
        
        let response = await this.http.put(`${this.apiUrl}(${this.id})`, record);

        if (response.isSuccess) {
            $.notify({ message: this.translations.updateRecordSuccess, icon: 'fa fa-check' }, { type: 'success' });
        }
        else {
            $.notify({ message: this.translations.updateRecordError, icon: 'fa fa-exclamation-triangle' }, { type: 'danger' });
        }
        
        this.refreshGrid();
        this.sectionSwitcher.swap('grid-section');
    }

    cancel() {
        this.sectionSwitcher.swap('grid-section');
    }

    refreshGrid() {
        this.grid.dataSource.read();
        this.grid.refresh();
    }
}