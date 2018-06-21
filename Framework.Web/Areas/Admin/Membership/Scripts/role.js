import 'jquery';
import 'jquery-validation';
import 'bootstrap-notify';

export class RoleViewModel {

    constructor(parent) {
        this.parent = parent;

        this.datasource = {
            type: 'odata-v4',
            transport: {
                read: this.parent.roleApiUrl
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
    }

    init() {
        this.validator = $("#roles-form-section-form").validate({
            rules: {
                Name: { required: true, maxlength: 255 }
            }
        });
    }

    create() {
        this.id = this.parent.emptyGuid;
        this.name = null;
        this.permissions = [];

        this.validator.resetForm();
        $("#roles-form-section-legend").html(this.parent.translations.create);
        this.parent.sectionSwitcher.swap('roles-form-section');
    }

    async edit(id) {
        let response = await this.parent.http.get(`${this.parent.roleApiUrl}('${id}')`);
        let entity = response.content;

        this.id = entity.Id;
        this.name = entity.Name;
        this.permissions = [];

        this.validator.resetForm();
        $("#roles-form-section-legend").html(this.parent.translations.edit);
        this.parent.sectionSwitcher.swap('roles-form-section');
    }

    async remove(id) {
        if (confirm(this.parent.translations.deleteRecordConfirm)) {
            let response = await this.parent.http.delete(`${this.parent.roleApiUrl}('${id}')`);
            if (response.isSuccess) {
                $.notify({ message: this.parent.translations.deleteRecordSuccess, icon: 'fa fa-check' }, { type: 'success' });
            }
            else {
                $.notify({ message: this.parent.translations.deleteRecordError, icon: 'fa fa-exclamation-triangle' }, { type: 'danger' });
            }

            this.refreshGrid();
        }
    }

    async save() {
        if (!$("#roles-form-section-form").valid()) {
            return false;
        }

        let isNew = (this.id == this.parent.emptyGuid);

        let record = {
            Id: this.id,
            Name: this.name
        };

        if (isNew) {
            let response = await this.parent.http.post(this.parent.roleApiUrl, record);
            //console.log('response: ' + JSON.stringify(response));
            if (response.isSuccess) {
                $.notify({ message: this.parent.translations.insertRecordSuccess, icon: 'fa fa-check' }, { type: 'success' });
            }
            else {
                $.notify({ message: this.parent.translations.insertRecordError, icon: 'fa fa-exclamation-triangle' }, { type: 'danger' });
            }
        }
        else {
            let response = await this.parent.http.put(`${this.parent.roleApiUrl}('${this.id}')`, record);
            //console.log('response: ' + JSON.stringify(response));
            if (response.isSuccess) {
                $.notify({ message: this.parent.translations.updateRecordSuccess, icon: 'fa fa-check' }, { type: 'success' });
            }
            else {
                $.notify({ message: this.parent.translations.updateRecordError, icon: 'fa fa-exclamation-triangle' }, { type: 'danger' });
            }
        }

        this.refreshGrid();
        this.parent.sectionSwitcher.swap('roles-grid-section');
    }

    cancel() {
        this.parent.sectionSwitcher.swap('roles-grid-section');
    }

    refreshGrid() {
        this.grid.dataSource.read();
        this.grid.refresh();
    }

    async editPermissions(id) {
        this.id = id;
        this.permissions = [];

        let response = await this.parent.http.get(`${this.parent.permissionsApiUrl}/Default.GetPermissionsForRole(roleId='${id}')`);
        let json = response.content;

        let self = this;
        if (json && json.length > 0) {
            $.each(json, function () {
                self.permissions.push(this.id);
            });
        }

        this.parent.sectionSwitcher.swap('role-permissions-form-section');
    }

    async editPermissions_save() {
        let record = {
            roleId: this.id,
            permissions: this.permissions
        };

        let response = await this.parent.http.post(`${this.parent.roleApiUrl}/Default.AssignPermissionsToRole`, record);

        if (response.isSuccess) {
            $.notify({ message: this.parent.translations.savePermissionsSuccess, icon: 'fa fa-check' }, { type: 'success' });
        }
        else {
            $.notify({ message: this.parent.translations.savePermissionsError, icon: 'fa fa-exclamation-triangle' }, { type: 'danger' });
        }

        this.parent.sectionSwitcher.swap('roles-grid-section');
    }

    editPermissions_cancel() {
        this.parent.sectionSwitcher.swap('roles-grid-section');
    }
}