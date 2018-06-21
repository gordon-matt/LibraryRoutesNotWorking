import $ from 'jquery';
import { HttpClient } from 'aurelia-http-client';

import { GenericHttpInterceptor } from '/aurelia-app/embedded/Framework.Web.CommonResources.Scripts.generic-http-interceptor';
import { SectionSwitcher } from '/aurelia-app/embedded/Framework.Web.CommonResources.Scripts.section-switching';

import { UserViewModel } from '/aurelia-app/embedded/Framework.Web.Areas.Admin.Membership.Scripts.user';
import { RoleViewModel } from '/aurelia-app/embedded/Framework.Web.Areas.Admin.Membership.Scripts.role';
import { ChangePasswordViewModel } from '/aurelia-app/embedded/Framework.Web.Areas.Admin.Membership.Scripts.change-password';

export class ViewModel {
    userApiUrl = "/odata/framework/web/UserApi";
    roleApiUrl = "/odata/framework/web/RoleApi";
    permissionsApiUrl = "/odata/framework/web/PermissionApi";

    emptyGuid = '00000000-0000-0000-0000-000000000000';

    constructor() {
        this.userModel = new UserViewModel(this);
        this.roleModel = new RoleViewModel(this);
        this.changePasswordModel = new ChangePasswordViewModel(this);

        this.http = new HttpClient();
        this.http.configure(config => {
            config.withInterceptor(new GenericHttpInterceptor());
        });
    }

    // Aurelia Component Lifecycle Methods

    async attached() {
        // Load translations first, else will have errors
        let response = await this.http.get("/admin/membership/get-translations");
        this.translations = response.content;
        
        this.sectionSwitcher = new SectionSwitcher('users-grid-section');
        
        this.roleModel.init();
        this.changePasswordModel.init();
        this.userModel.init();
    }

    // END: Aurelia Component Lifecycle Methods

    viewUsers() {
        this.sectionSwitcher.swap('users-grid-section');
    }

    viewRoles() {
        this.sectionSwitcher.swap('roles-grid-section');
    }
}