import 'jquery';
import 'jquery-validation';
import 'bootstrap';
import 'bootstrap-fileinput';
import 'bootstrap-notify';
import { HttpClient } from 'aurelia-http-client';

import { GenericHttpInterceptor } from '/aurelia-app/embedded/Framework.Web.CommonResources.Scripts.generic-http-interceptor';
import { SectionSwitcher } from '/aurelia-app/embedded/Framework.Web.CommonResources.Scripts.section-switching';

export class ViewModel {
    apiUrl = "/odata/framework/web/LanguageApi";
    emptyGuid = '00000000-0000-0000-0000-000000000000';
    
    constructor() {
        this.datasource = {
            type: 'odata-v4',
            transport: {
                read: this.apiUrl
            },
            schema: {
                model: {
                    fields: {
                        Name: { type: "string" },
                        CultureCode: { type: "string" },
                        IsEnabled: { type: "boolean" },
                        SortOrder: { type: "number" }
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
        // Load translations first, else will have errors
        let response = await this.http.get("/admin/localization/languages/get-translations");
        this.translations = response.content;

        this.datasource.pageSize = $("#GridPageSize").val();

        this.sectionSwitcher = new SectionSwitcher('grid-section');

        this.validator = $("#form-section-form").validate({
            rules: {
                Name: { required: true, maxlength: 255 },
                CultureCode: { required: true, maxlength: 10 },
                SortOrder: { required: true }
            }
        });

        $("#Upload").fileinput({
            uploadUrl: '/admin/localization/languages/import-language-pack',
            uploadAsync: false,
            maxFileCount: 1,
            showPreview: false,
            showRemove: false,
            allowedFileExtensions: ['json']
        });

        let self = this;
        $('#Upload').on('filebatchuploadsuccess', function (event, data) {
            var response = data.response;
            self.refreshGrid();
            self.sectionSwitcher.swap('grid-section');
            $.notify({ message: response.message, icon: 'fa fa-check' }, { type: 'success' });
        });
    }

    // END: Aurelia Component Lifecycle Methods

    create() {
        this.id = this.emptyGuid;
        this.name = null;
        this.cultureCode = null;
        this.isRTL = false;
        this.isEnabled = false;
        this.sortOrder = 0;
        
        this.validator.resetForm();
        $("#form-section-legend").html(this.translations.create);
        this.sectionSwitcher.swap('form-section');
    }

    async edit(id) {
        let response = await this.http.get(`${this.apiUrl}(${id})`);
        let entity = response.content;
        
        this.id = entity.Id;
        this.name = entity.Name;
        this.cultureCode = entity.CultureCode;
        this.isRTL = entity.IsRTL;
        this.isEnabled = entity.IsEnabled;
        this.sortOrder = entity.SortOrder;

        this.validator.resetForm();
        $("#form-section-legend").html(this.translations.edit);
        this.sectionSwitcher.swap('form-section');
    }

    async remove(id) {
        if (confirm(this.translations.deleteRecordConfirm)) {
            let response = await this.http.delete(`${this.apiUrl}(${id})`);
            if (response.isSuccess) {
                $.notify({ message: this.translations.deleteRecordSuccess, icon: 'fa fa-check' }, { type: 'success' });
            }
            else {
                $.notify({ message: this.translations.deleteRecordError, icon: 'fa fa-exclamation-triangle' }, { type: 'danger' });
            }

            this.refreshGrid();
        }
    }

    async save() {
        if (!$("#form-section-form").valid()) {
            return false;
        }

        let isNew = (this.id == this.emptyGuid);

        let record = {
            Id: this.id,
            Name: this.name,
            CultureCode: this.cultureCode,
            IsRTL: this.isRTL,
            IsEnabled: this.isEnabled,
            SortOrder: this.sortOrder,
            Hosts: this.hosts
        };

        if (isNew) {
            let response = await this.http.post(this.apiUrl, record);
            //console.log('response: ' + JSON.stringify(response));
            if (response.isSuccess) {
                $.notify({ message: this.translations.insertRecordSuccess, icon: 'fa fa-check' }, { type: 'success' });
            }
            else {
                $.notify({ message: this.translations.insertRecordError, icon: 'fa fa-exclamation-triangle' }, { type: 'danger' });
            }
        }
        else {
            let response = await this.http.put(`${this.apiUrl}(${this.id})`, record);
            //console.log('response: ' + JSON.stringify(response));
            if (response.isSuccess) {
                $.notify({ message: this.translations.updateRecordSuccess, icon: 'fa fa-check' }, { type: 'success' });
            }
            else {
                $.notify({ message: this.translations.updateRecordError, icon: 'fa fa-exclamation-triangle' }, { type: 'danger' });
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

    async clear() {
        let response = await this.http.post(`${this.apiUrl}/Default.ResetLocalizableStrings`);

        if (response.isSuccess) {
            $.notify({ message: this.translations.resetLocalizableStringsSuccess, icon: 'fa fa-check' }, { type: 'success' });
            setTimeout(function () {
                window.location.reload();
            }, 500);
        }
        else {
            $.notify({ message: this.translations.resetLocalizableStringsError, icon: 'fa fa-exclamation-triangle' }, { type: 'danger' });
        }
    }

    importLanguagePack() {
        this.sectionSwitcher.swap('upload-section');
    }
}