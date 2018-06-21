import 'jquery';
import 'jquery-validation';
import 'bootstrap-notify';

export class UserViewModel {

    constructor(parent) {
        this.parent = parent;

        this.datasource = {
            type: 'odata-v4',
            transport: {
                read: this.parent.userApiUrl
            },
            schema: {
                model: {
                    fields: {
                        UserName: { type: "string" },
                        Email: { type: "string" },
                        IsLockedOut: { type: "boolean" }
                    }
                }
            },
            pageSize: 10,
            serverPaging: true,
            serverFiltering: true,
            serverSorting: true,
            sort: { field: "UserName", dir: "asc" }
        };
    }

    init() {
        this.validator = $("#users-edit-form-section-form").validate({
            rules: {
                UserName: { required: true, maxlength: 128 },
                Email: { required: true, maxlength: 255 },
            }
        });
    }

    create() {
        this.id = this.parent.emptyGuid;
        this.userName = null;
        this.email = null;
        this.isLockedOut = false;

        this.roles = [];
        this.filterRoleId = this.parent.emptyGuid;

        this.validator.resetForm();
        this.parent.sectionSwitcher.swap('users-edit-form-section');
    }

    async edit(id) {
        let response = await this.parent.http.get(`${this.parent.userApiUrl}('${id}')`);
        let entity = response.content;

        this.id = entity.Id;
        this.userName = entity.UserName;
        this.email = entity.Email;
        this.isLockedOut = entity.IsLockedOut;

        this.roles = [];
        this.filterRoleId = this.parent.emptyGuid;

        this.validator.resetForm();
        this.parent.sectionSwitcher.swap('users-edit-form-section');
    }

    async remove(id) {
        if (confirm(this.parent.translations.deleteRecordConfirm)) {
            let response = await this.parent.http.delete(`${this.parent.userApiUrl}('${id}')`);
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
        if (!$("#users-edit-form-section-form").valid()) {
            return false;
        }

        let isNew = (this.id == this.parent.emptyGuid);

        let record = {
            Id: this.id,
            UserName: this.userName,
            Email: this.email,
            IsLockedOut: this.isLockedOut
        };

        if (isNew) {
            let response = await this.parent.http.post(this.parent.userApiUrl, record);
            if (response.isSuccess) {
                $.notify({ message: this.parent.translations.insertRecordSuccess, icon: 'fa fa-check' }, { type: 'success' });
            }
            else {
                $.notify({ message: this.parent.translations.insertRecordError, icon: 'fa fa-exclamation-triangle' }, { type: 'danger' });
            }
        }
        else {
            let response = await this.parent.http.put(`${this.parent.userApiUrl}('${this.id}')`, record);
            if (response.isSuccess) {
                $.notify({ message: this.parent.translations.updateRecordSuccess, icon: 'fa fa-check' }, { type: 'success' });
            }
            else {
                $.notify({ message: this.parent.translations.updateRecordError, icon: 'fa fa-exclamation-triangle' }, { type: 'danger' });
            }
        }

        this.refreshGrid();
        this.parent.sectionSwitcher.swap('users-grid-section');
    }

    cancel() {
        this.parent.sectionSwitcher.swap('users-grid-section');
    }

    refreshGrid() {
        this.grid.dataSource.read();
        this.grid.refresh();
    }

    async editRoles(id) {
        this.id = id;
        this.roles = [];
        this.filterRoleId = this.parent.emptyGuid;

        let response = await this.parent.http.get(`${this.parent.roleApiUrl}/Default.GetRolesForUser(userId='${id}')`);
        let json = response.content;

        let self = this;
        if (json && json.length > 0) {
            $.each(json, function () {
                self.roles.push(this.id);
            });
        }

        this.parent.sectionSwitcher.swap('user-roles-form-section');
    }

    async editRoles_save() {
        var record = {
            userId: this.id,
            roles: this.roles
        };

        let response = await this.parent.http.post(`${this.parent.userApiUrl}/Default.AssignUserToRoles`, record);
        if (response.isSuccess) {
            $.notify({ message: this.parent.translations.saveRolesSuccess, icon: 'fa fa-check' }, { type: 'success' });
        }
        else {
            $.notify({ message: this.parent.translations.saveRolesError, icon: 'fa fa-exclamation-triangle' }, { type: 'danger' });
        }

        this.parent.sectionSwitcher.swap('users-grid-section');
    }

    editRoles_cancel() {
        this.parent.sectionSwitcher.swap('users-grid-section');
    }

    changePassword(id, userName) {
        this.parent.changePasswordModel.id = id;
        this.parent.changePasswordModel.userName = userName;

        this.parent.changePasswordModel.validator.resetForm();
        this.parent.sectionSwitcher.swap('change-password-form-section');
    }

    filterRole() {
        if (this.filterRoleId == "") {
            this.grid.dataSource.transport.options.read.url = this.parent.userApiUrl;
        }
        else {
            this.grid.dataSource.transport.options.read.url = `${this.parent.userApiUrl}/Default.GetUsersInRole(roleId='${this.filterRoleId}')`;
        }
        this.grid.dataSource.page(1);
    }
}