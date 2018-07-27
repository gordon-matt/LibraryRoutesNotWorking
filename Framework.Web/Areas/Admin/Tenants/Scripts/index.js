import 'jquery';
import 'jquery-validation';
import 'bootstrap-notify';
import { HttpClient } from 'aurelia-http-client';

import { GenericHttpInterceptor } from '/aurelia-app/embedded/Framework.Web.CommonResources.Scripts.generic-http-interceptor';
import { SectionSwitcher } from '/aurelia-app/embedded/Framework.Web.CommonResources.Scripts.section-switching';

export class ViewModel {
    apiUrl = "/odata/framework/web/TenantApi";

    constructor() {
        this.datasource = {
            type: 'odata-v4',
            transport: {
                read: this.apiUrl
            },
            schema: {
                model: {
                    fields: {
                        Name: { type: "string" }
                    }
                }
            },
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
        this.sectionSwitcher = new SectionSwitcher('grid-section');

        this.validator = $("#form-section-form").validate({
            rules: {
                Name: { required: true, maxlength: 255 },
                Url: { required: true, maxlength: 255 },
                Hosts: { required: true }
            }
        });
    }

    // END: Aurelia Component Lifecycle Methods

    create() {
        this.id = 0;
        this.name = null;
        this.url = null;
        this.hosts = null;

        this.validator.resetForm();
        $("#form-section-legend").html("Create");
        this.sectionSwitcher.swap('form-section');
    }

    async edit(id) {
        let response = await this.http.get(`${this.apiUrl}(${id})`);
        let entity = response.content;

        this.id = entity.Id;
        this.name = entity.Name;
        this.url = entity.Url;
        this.hosts = entity.Hosts;

        this.validator.resetForm();
        $("#form-section-legend").html("Edit");
        this.sectionSwitcher.swap('form-section');
    }

    async remove(id) {
        if (confirm("Are you sure you want to delete this record?")) {
            let response = await this.http.delete(`${this.apiUrl}(${id})`);
            if (response.isSuccess) {
                $.notify({ message: "Successfully deleted record!", icon: 'fa fa-check' }, { type: 'success' });
            }
            else {
                $.notify({ message: "There was a problem when trying to delete the record.", icon: 'fa fa-exclamation-triangle' }, { type: 'danger' });
            }

            this.refreshGrid();
        }
    }

    async save() {
        if (!$("#form-section-form").valid()) {
            return false;
        }

        let isNew = (this.id == 0);

        let record = {
            Id: this.id,
            Name: this.name,
            Url: this.url,
            Hosts: this.hosts
        };

        if (isNew) {
            let response = await this.http.post(this.apiUrl, record);
            //console.log('response: ' + JSON.stringify(response));
            if (response.isSuccess) {
                $.notify({ message: "Successfully inserted record!", icon: 'fa fa-check' }, { type: 'success' });
            }
            else {
                $.notify({ message: "There was a problem when trying to insert the record.", icon: 'fa fa-exclamation-triangle' }, { type: 'danger' });
            }
        }
        else {
            let response = await this.http.put(`${this.apiUrl}(${this.id})`, record);
            //console.log('response: ' + JSON.stringify(response));
            if (response.isSuccess) {
                $.notify({ message: "Successfully updated record!", icon: 'fa fa-check' }, { type: 'success' });
            }
            else {
                $.notify({ message: "There was a problem when trying to update the record.", icon: 'fa fa-exclamation-triangle' }, { type: 'danger' });
            }
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