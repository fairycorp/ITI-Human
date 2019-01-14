<template>
    <div class="main">
        <div class="left-page" :class="{ invisible : WINDOW_PROJECT_SETUP || WINDOW_PROJECT_ADD_MEMBER || WINDOW_PROJECT_INVENTORY || WINDOW_PROJECT_ACCOUNTS }">
            <h1>Liste de vos projets</h1>
            <div class="cover-photo"></div>
            
            <div class="create-after">
                <button @click="launchProjectSetup()" class="standard">CREER UN PROJET</button>
            </div>
            <div class="content">
                <h3 class="pretitle">À L'AFFICHE</h3>
                <div v-if="displayedProject !== null && displayedProject !== undefined">
                    <div class="project-title">{{ displayedProject.projectName }}</div>

                    <div v-if="displayedProject.members.length > 0" class="memberlist">
                        <div v-for="member in displayedProject.members" :key="member.projectMemberId" class="member">
                            <img 
                                v-if="member.avatarUrl !== null"
                                :src="member.avatarUrl" class="member-avatar"
                                :class="{ chief : member.projectRankId === 1 }" />
                            <img
                                v-else
                                src="../assets/images/unknown-user.png"
                                class="member-avatar"
                                :class="{ chief : member.projectRankId === 1 }" />
                        </div>
                        <img
                            v-if="currentUserSchoolProfile.projectRankId === 1"
                            @click="launchProjectAddMember()"
                            class="member-avatar add-member-button"
                            src="../assets/images/add-button.png" />
                    </div>

                    <div class="medium-top-margin headline">
                        <div class="subtitle"><span class="openSans-bold">SLOGAN</span> DU PROJET</div>
                        <div class="headline-content">"{{ displayedProject.projectHeadline }}"</div>
                    </div>

                    <div class="medium-top-margin pitch">
                        <div class="subtitle"><span class="openSans-bold">PITCH</span> DU PROJET</div>
                        "{{ displayedProject.projectPitch }}"
                    </div>

                    <div v-if="userProjects.length > 1" class="nav">
                        <button
                            v-if="displayedProject !== userProjects[0]"
                            @click="changeCurrentDisplayedProject(-1)"
                            class="light-right-margin navigation"> < </button>
                        <button
                            v-if="displayedProject !== userProjects[userProjects.length - 1]"
                            @click="changeCurrentDisplayedProject(1)"
                            class="navigation"> > </button>
                    </div>

                    <div class="action-buttons">
                        <div :class="{ inventorysidetext : WINDOW_PROJECT_INVENTORY, hidden : !WINDOW_PROJECT_INVENTORY }"></div>
                        <button
                            v-if="displayedProject.semesterId === 4"
                            @click="launchProjectInventory()"
                            :class="{ inventoryselected : WINDOW_PROJECT_INVENTORY }"
                            class="bottom-button inventory"></button>
                        <button
                            v-if="displayedProject.semesterId === 4"
                            @click="launchProjectAccounts()"
                            :class="{ accountsselected : WINDOW_PROJECT_ACCOUNTS }"
                            class="bottom-button accounts"></button>
                        <div :class="{ accountssidetext : WINDOW_PROJECT_ACCOUNTS, hidden : !WINDOW_PROJECT_ACCOUNTS }"></div>
                    </div>
                </div>
                <div v-else>
                    <div class="project-title">Absolument rien.</div>
                </div>
            </div>
        </div>
        <div v-if="WINDOW_PROJECT_SETUP" class="right-page">
            <div @click="closeProjectSetupWindow()" class="cross">x</div>
            <h1>Création d'un projet</h1>
            <h3 class="medium-top-margin">SON IDENTITE</h3>
            <input
                v-model="projectName"
                type="text"
                :class="{ error : FIELD_PROJECTNAME_ERROR }"
                class="light-top-margin textual"
                placeholder="Son petit nom"
            />

            <input
                v-model="projectHeadline"
                type="text"
                :class="{ error : FIELD_PROJECTHEADLINE_ERROR }"
                class="medium-top-margin textual"
                placeholder="Un éventuel slogan ?"
            />

            <input
                v-model="projectPitch"
                type="text"
                :class="{ error : FIELD_PROJECTPITCH_ERROR }"
                class="medium-top-margin textual"
                placeholder="Un p'tit pitch ?"
            />

            <h3 class="high-top-margin">SON SEMESTRE</h3>
            <button
                @click="selectSemester(1)"
                :class="{ selectedButton: selectedSemester === 1, error: FIELD_SEMESTER_ERROR }" 
                class="selection light-right-margin">S1</button>
            <button
                @click="selectSemester(2)"
                :class="{ selectedButton: selectedSemester === 2, error: FIELD_SEMESTER_ERROR }" 
                class="selection light-right-margin">S2</button>
            <button
                @click="selectSemester(3)"
                :class="{ selectedButton: selectedSemester === 3, error: FIELD_SEMESTER_ERROR }" 
                class="selection light-right-margin">S3</button>
            <button
                @click="selectSemester(4)"
                :class="{ selectedButton: selectedSemester === 4, error: FIELD_SEMESTER_ERROR }" 
                class="selection">S4</button>

            <h3 class="high-top-margin">SES MEMBRES</h3>
            <input
                v-model="searchBarContent"
                v-on:keyup="searchUser(searchBarContent)"
                type="text"
                :class="{ error: FIELD_SEARCHBARCONTENT_ERROR }"
                class="light-top-margin textual"
                placeholder="Pseudo/nom du membre"
            />
            <div v-if="searchResult" @click="addUser(searchResult)" class="light-top-margin searchResult">
                <img v-if="searchResult.avatarUrl !== null" width="50" :src="searchResult.avatarUrl" class="avatar light-right-margin" />
                <img v-else width="50" src="../assets/images/unknown-user.png" class="avatar light-right-margin" />
                {{ searchResult.firstName }} <span class="openSans-bold">{{ searchResult.lastName }}</span>, {{ searchResult.userName }}
                <div class="add-button">AJOUTER</div>
            </div>
            <div v-if="projectUserList.length > 0" class="medium-top-margin projectUserList">
                <div class="minititle">
                    <span class="openSans-bold">{{ projectUserList.length }}</span>
                    membre<span v-if="projectUserList.length > 1">s</span>
                    récemment ajouté<span v-if="projectUserList.length > 1">s</span>.
                </div>
                <span v-for="member in projectUserList" :key="member.userId">
                    {{ member.firstName }} <span class="openSans-bold">{{ member.lastName }}</span> ({{ member.userName }})<span v-if="member == projectUserList[projectUserList.length - 1]">.</span><span v-else>, </span>
                </span>
            </div>
            <div
                class="special submit-button">
                <div
                    @click="submitProjectSetup()"
                    class="submit-button-text unselectable-text">
                    ALLEZ-Y LES GARS, CREEZ MON PROJET !
                </div>
            </div>
        </div>
        <div v-if="displayedProject !== null && displayedProject !== undefined && WINDOW_PROJECT_ADD_MEMBER" class="light-right-page">
            <div @click="closeProjectAddMemberWindow()" class="cross">x</div>
            <h1>Ajouter un membre</h1>
            <input
                v-model="searchBarContent"
                v-on:keyup="searchUser(searchBarContent)"
                type="text"
                :class="{ error: FIELD_SEARCHBARCONTENT_ERROR }"
                class="light-top-margin textual"
                placeholder="Pseudo/nom du membre"
            />
            <div v-if="searchResult" @click="addNewUserToExistingProject(searchResult)" class="light-top-margin searchResult">
                <img v-if="searchResult.avatarUrl !== null" width="50" :src="searchResult.avatarUrl" class="avatar light-right-margin" />
                <img v-else width="50" src="../assets/images/unknown-user.png" class="avatar light-right-margin" />
                {{ searchResult.firstName }} <span class="openSans-bold">{{ searchResult.lastName }}</span>, {{ searchResult.userName }}
                <div class="add-button">AJOUTER</div>
            </div>
            <div v-if="TEXT_ERROR_SEARCHBARCONTENT" class="light-top-margin infotext">{{ TEXT_ERROR_SEARCHBARCONTENT }}</div>
        </div>
        <div v-if="displayedProject !== null && displayedProject !== undefined && WINDOW_PROJECT_INVENTORY" class="right-page">
            <div @click="closeProjectInventoryWindow()" class="cross">x</div>
            <h1>Inventaire du projet</h1>
            <div v-if="currentUserSchoolProfile.projectRankId === 1">
                <h3 class="medium-top-margin">AJOUTER UN PRODUIT</h3>

                <input
                    v-model="productSearchBarContent"
                    v-on:keyup="searchProduct(productSearchBarContent)"
                    type="text"
                    class="textual short"
                    placeholder="Nom du produit"
                />
                <input
                    @keyup.enter="addNewProductToExistingInventory(productSearchResult)"
                    v-model="productPrice"
                    type="number"
                    min="0"
                    step="0.1"
                    :class="{ error : FIELD_PRODUCTPRICE_ERROR }"
                    class="textual extra-short"
                    placeholder="Prix"
                />
                <input
                    @keyup.enter="addNewProductToExistingInventory(productSearchResult)"
                    v-model="productQuantity"
                    type="number"
                    min="0"
                    step="0.1"
                    :class="{ error : FIELD_PRODUCTQUANTITY_ERROR }"
                    class="textual extra-short"
                    placeholder="Qté"
                />
                <div v-if="productSearchResult" @click="addNewProductToExistingInventory(productSearchResult)" class="light-top-margin searchResult">
                    <img width="60" :src="productSearchResult.url" class="avatar light-right-margin" />
                    <span class="openSans-bold">{{ productSearchResult.name }}</span>
                    <div class="add-button">AJOUTER
                        <span v-if="productQuantity !== null || productQuantity !== undefined || productQuantity !== ''">(</span>
                        <span v-if="productQuantity !== null && productQuantity !== undefined && productQuantity !== ''">{{ productQuantity }} {{ productSearchResult.productName }}</span>
                        <span v-if="productPrice !== null && productPrice !== undefined && productPrice !== ''">à {{ productPrice }}€/unité</span>
                        <span v-if="productQuantity !== null || productQuantity !== undefined || productQuantity !== ''">)</span>
                    </div>
                </div>
                <div v-if="TEXT_ERROR_PRODUCTSEARCHBAR" class="light-top-margin infotext">{{ TEXT_ERROR_PRODUCTSEARCHBAR }}</div>
            </div>

            <h3
                :class="{ lightmargintop : currentUserSchoolProfile.projectRankId === 0, highmargintop : currentUserSchoolProfile.projectRankId === 1 }">
                LISTE DES PRODUITS
            </h3>
            <div>
                <div v-for="slp in slpList" :key="slp.storageLinkedProductId" class="light-top-margin">
                    <img width="50" class="avatar light-right-margin" :src="slp.productAvatarUrl" />
                    <span class="openSans-bold">{{ slp.stock }}</span><span class="productname"> {{ slp.productName}}</span> à <span class="openSans-bold">{{ slp.unitPrice / 100 }}</span>€/unité.
                </div>
            </div>
        </div>

        <div v-if="displayedProject !== null && displayedProject !== undefined && WINDOW_PROJECT_ACCOUNTS" class="right-page">
            <div @click="closeProjectAccountsWindow()" class="cross">x</div>
            <h1>Consultation des comptes</h1>
            <div class="info-account-title">
                <span v-if="totalBalance > 0">Vous devez</span><span v-else>On vous doit</span>
                à ce jour <span :class="{ badBalance: totalBalance > 0, goodBalance: totalBalance < 0 }" class="openSans-bold">{{ displayedTotalBalance / 100 }} €</span>
            </div>
        </div>
    </div>
</template>

<script lang="ts">
import { Component, Prop, Vue, Watch } from "vue-property-decorator";
import { appSettings } from "@/config/appSettings";
import {
  AuthService,
  IAuthServiceConfiguration,
AuthLevel
} from "@signature/webfrontauth";
import Axios from "axios";
import API from "@/services/API";
import Endpoint from "@/helpers/Endpoint";

import {
    IBasicDataProject,
    IProjectCreationViewModel,
    IBasicDataProjectMember,
    IProjectMemberCreationViewModel
} from "@/models/model.Project";
import { ESStatus } from "@/models/model.SchoolStatus";
import { ESemester } from "@/models/model.Semester";
import { IDetailedDataUser } from "@/models/model.User";
import {
    IBasicDataStorageLinkedProduct,
    IStorageCreationViewModel,
    ILinkedProductCreationViewModel
} from "@/models/model.Storage";
import { IBasicDataProduct } from "@/models/model.Product";
import { IUserBalance } from "@/models/model.Order";

@Component({})
export default class ProjectsDB extends Vue {
    @Prop() private authService!: AuthService;
    @Prop() private displayedProject!: IBasicDataProject;
    @Prop() private currentUserSchoolProfile!: IBasicDataProjectMember;
    private userProjects: IBasicDataProject[] = [];
    private userList: IDetailedDataUser[] = [];
    private productList: IBasicDataProduct[] = [];
    private slpList: IBasicDataStorageLinkedProduct[] = [];
    private balanceList: IUserBalance[] = [];
    private totalBalance: number = 0;
    private displayedTotalBalance: string = "";
    private projectName: string = "";
    private projectHeadline: string = "";
    private projectPitch: string = "";
    private selectedSemester: number = 0;
    private searchBarContent: string = "";
    private searchResult: IDetailedDataUser | null = null;
    private productSearchBarContent: string = "";
    private productSearchResult: IBasicDataProduct | null = null;
    private productPrice: number | null = null;
    private productQuantity: number | null = null;
    private projectUserList: IDetailedDataUser[] = [];
    private WINDOW_PROJECT_SETUP: boolean = false;
    private WINDOW_PROJECT_ADD_MEMBER: boolean = false;
    private WINDOW_PROJECT_INVENTORY: boolean = false;
    private WINDOW_PROJECT_ACCOUNTS: boolean = false;
    private FIELD_PROJECTNAME_ERROR: boolean = false;
    private FIELD_PROJECTHEADLINE_ERROR: boolean = false;
    private FIELD_PROJECTPITCH_ERROR: boolean = false;
    private FIELD_SEMESTER_ERROR: boolean = false;
    private FIELD_SEARCHBARCONTENT_ERROR: boolean = false;
    private FIELD_PRODUCTPRICE_ERROR: boolean = false;
    private FIELD_PRODUCTQUANTITY_ERROR: boolean = false;
    private TEXT_ERROR_SEARCHBARCONTENT: string = "";
    private TEXT_ERROR_PRODUCTSEARCHBAR: string = "";

    constructor() {
        super();
        this.isAccessible();
        this.fetchUserProjects(0);
        this.fetchUserList();
    }

    // DATACHECKING METHODS.
    /** Checks if the current view route can be accessed. */
    private isAccessible() {
        if (Vue.prototype.$authService === undefined) {
            this.$router.push("/");
        } else {
            this.authService = Vue.prototype.$authService;
            this.authService.refresh(true, true, true);

            if (this.authService.authenticationInfo.level === 0) {
                this.$router.push("/");
            }
        }
    }

    private inputDataCheck(inputData: string, length: number): boolean {
        if (inputData !== null && inputData !== undefined) {
            if (inputData.length < length) {
                return false;
            }
            return true;
        }
        return false;
    }

    // SELECTION TOOLING METHODS.
    private selectSemester(semesterId: number) {
        const newSemester = semesterId as ESemester;
        switch (newSemester) {
            case (ESemester.S1):
                this.selectedSemester = ESemester.S1 as number;
                this.FIELD_SEMESTER_ERROR = false;
                break;

            case (ESemester.S2):
                this.selectedSemester = ESemester.S2 as number;
                this.FIELD_SEMESTER_ERROR = false;
                break;

            case (ESemester.S3):
                this.selectedSemester = ESemester.S3 as number;
                this.FIELD_SEMESTER_ERROR = false;
                break;

            case (ESemester.S4):
                this.selectedSemester = ESemester.S4 as number;
                this.FIELD_SEMESTER_ERROR = false;
                break;
        }
    }

    private addUser(user: IDetailedDataUser) {
        if (this.projectUserList.length > 0) {
            let match = false;
            this.projectUserList.forEach( (registered) => {
                if (registered.userId === user.userId) {
                    match = true;
                }
            });
            if (match) return;
            else this.projectUserList.push(user);
        } else {
            this.projectUserList.push(user);
        }
        this.searchResult = null;
        this.searchBarContent = "";
    }

    private async addNewUserToExistingProject(user: IDetailedDataUser) {
        this.displayedProject.members.forEach( (member) => {
            if (member.userId === user.userId) {
                this.FIELD_SEARCHBARCONTENT_ERROR = true;
                this.TEXT_ERROR_SEARCHBARCONTENT =
                    `${member.userName} est déjà dans la liste des membres de ce projet.`;
                return;
            }
        });

        const payload: IProjectMemberCreationViewModel = {
            projectId: this.displayedProject.projectId,
            userId: user.userId
        };
        const response = await API.post(`${Endpoint.Project}/member/add`, payload);
        if (response.data) {
            this.closeProjectAddMemberWindow();
            this.fetchUserProjects(this.displayedProject.projectId);
        }
    }

    private async addNewProductToExistingInventory(product: IBasicDataProduct) {
        if (this.productQuantity === null
        || this.productQuantity === undefined
        || this.productPrice === null
        || this.productPrice === undefined
        || typeof this.productQuantity === "number"
        || typeof this.productPrice === "number"
        || this.productQuantity == 0
        || this.productPrice == 0
        || this.productQuantity < 0
        || this.productPrice < 0) {
            if (this.productPrice === null || this.productPrice === undefined) {
                this.FIELD_PRODUCTPRICE_ERROR = true;
            }

            if (this.productQuantity === null || this.productQuantity === undefined) {
                this.FIELD_PRODUCTQUANTITY_ERROR = true;
            }

            if (this.productQuantity == 0) {
                this.FIELD_PRODUCTQUANTITY_ERROR = true;
            }

            if (this.productPrice == 0) {
                this.FIELD_PRODUCTPRICE_ERROR = true;
            }

            this.TEXT_ERROR_PRODUCTSEARCHBAR = "Merci d'ajouter un prix et/ou une quantité.";
            return;
        } else {
            this.slpList.forEach( (slp) => {
                if (slp.productName === product.name) {
                    this.TEXT_ERROR_PRODUCTSEARCHBAR = "Ce produit existe déjà dans votre inventaire.";
                }
            });

            const payload: ILinkedProductCreationViewModel = {
                userId: this.authService.authenticationInfo.user.userId,
                storageId: this.displayedProject.storageId,
                productId: this.productSearchResult!.productId,
                unitPrice: this.productPrice * 100,
                stock: this.productQuantity
            };
            const response = await API.post(`${Endpoint.Storage}/products/create`, payload);
            if (response.data) {
                this.productSearchBarContent = "";
                this.productPrice = null;
                this.productQuantity = null;
                this.productSearchResult = null;
                this.FIELD_PRODUCTPRICE_ERROR = false;
                this.FIELD_PRODUCTQUANTITY_ERROR = false;
                this.TEXT_ERROR_PRODUCTSEARCHBAR = "";
                this.fetchSLPlist();
                return;
            }
        }
    }

    // SEARCHING METHODS.
    private searchUser(username: string) {
        if (username.length > 0) {
            this.userList.forEach( (user) => {
                if (user.userId !== this.authService.authenticationInfo.user.userId) {
                    if (user.userName.toLowerCase().startsWith(username.toLowerCase())
                    || user.firstName.toLowerCase().startsWith(username.toLowerCase())
                    || user.lastName.toLowerCase().startsWith(username.toLowerCase())) {
                        this.searchResult = user;
                    }
                }
            });
        } else {
            this.searchResult = null;
        }
    }

    private searchProduct(productname: string) {
        if (productname.length > 0) {
            this.productList.forEach( (product) => {
                    if (product.name.toLowerCase().startsWith(productname.toLowerCase())
                    || product.name.toLowerCase().endsWith(productname.toLowerCase())) {
                        this.productSearchResult = product;
                }
            });
        } else {
            this.productSearchResult = null;
        }
    }

    // GETTERS METHODS.
    private async fetchUserProjects(displayedProject: number) {
        const response = await API
            .get(`${Endpoint.Project}/u/${this.authService.authenticationInfo.user.userId}`);
        this.userProjects = response.data;

        this.userProjects.forEach( (project) => {
            if (project.semesterId === 4) {
                this.fetchProductList();
            }
        });

        this.changeCurrentDisplayedProjectByProjectId(displayedProject);

        this.displayedProject.members.forEach( (member) => {
            if (member.userId === this.authService.authenticationInfo.user.userId) {
                this.currentUserSchoolProfile = member;
            }
        });
    }

    private async fetchUserList() {
        const response = await API.get(`${Endpoint.User}/tooltip`);
        this.userList = response.data;
    }

    private async fetchProductList() {
        const response = await API.get(`${Endpoint.Product}`);
        this.productList = response.data;
    }

    private async fetchSLPlist() {
        const response = await API.get(`${Endpoint.Storage}/products/from/${this.displayedProject.storageId}`);
        this.slpList = response.data;
    }

    private async fetchBalanceList() {
        const response = await API.get(`${Endpoint.Order}/project/${this.displayedProject.projectId}/balances`);
        this.balanceList = response.data;

        this.totalBalance = 0;
        this.displayedTotalBalance = "";
        this.balanceList.forEach( (balance) => {
            this.totalBalance += balance.balance;
        });
        if (this.totalBalance.toString().startsWith("-")) {
            this.displayedTotalBalance = this.totalBalance.toString().split("-")[1];
        }
    }

    private changeCurrentDisplayedProjectByProjectId(projectId: number) {
        for (let index: number = 0; index < this.userProjects.length; index++) {
            if (this.userProjects[index].projectId === projectId) {
                this.displayedProject = this.userProjects[index];
                return;
            }
        }
        this.displayedProject = this.userProjects[0];
        this.fetchSLPlist();
        this.fetchBalanceList();
    }

    private changeCurrentDisplayedProject(newIndex: number) {
        for (let index: number = 0; index < this.userProjects.length; index++) {
            if (this.userProjects[index] === this.displayedProject) {
                this.displayedProject = this.userProjects[index + (newIndex)];
                this.fetchSLPlist();
                this.fetchBalanceList();
                return;
            }
        }
    }

    // WINDOW METHODS.
    private launchProjectSetup() {
        this.WINDOW_PROJECT_SETUP = true;
    }

    private launchProjectAddMember() {
        this.WINDOW_PROJECT_ADD_MEMBER = true;
    }

    private launchProjectInventory() {
        this.WINDOW_PROJECT_INVENTORY = true;
        this.fetchSLPlist();
    }

    private launchProjectAccounts() {
        this.WINDOW_PROJECT_ACCOUNTS = true;
        this.fetchBalanceList();
    }

    private closeProjectSetupWindow() {
        this.WINDOW_PROJECT_SETUP = false;
        this.searchResult = null;
        this.searchBarContent = "";
        this.projectName = "";
        this.projectHeadline = "";
        this.projectPitch = "";
        this.projectUserList = [];
        this.selectedSemester = 0;
    }

    private closeProjectAddMemberWindow() {
        this.WINDOW_PROJECT_ADD_MEMBER = false;
        this.searchResult = null;
        this.searchBarContent = "";
        this.FIELD_SEARCHBARCONTENT_ERROR = false;
        this.TEXT_ERROR_SEARCHBARCONTENT = "";
    }

    private closeProjectInventoryWindow() {
        this.WINDOW_PROJECT_INVENTORY = false;
        this.productSearchBarContent = "";
        this.productSearchResult = null;
        this.productPrice = null;
        this.productQuantity = null;
        this.TEXT_ERROR_PRODUCTSEARCHBAR = "";
        this.FIELD_PRODUCTPRICE_ERROR = false;
        this.FIELD_PRODUCTQUANTITY_ERROR = false;
    }

    private closeProjectAccountsWindow() {
        this.WINDOW_PROJECT_ACCOUNTS = false;
    }

    // SUBMITTING METHODS.
    private async submitProjectSetup() {
        let passed: boolean = true;
        const projectNameCheck = this.inputDataCheck(this.projectName, 2);
        const projectHeadlineCheck = this.inputDataCheck(this.projectHeadline, 5);
        const projectPitchCheck = this.inputDataCheck(this.projectPitch, 10);
        if (!projectNameCheck || !projectHeadlineCheck || !projectPitchCheck) {
            if (!projectNameCheck) this.FIELD_PROJECTNAME_ERROR = true;
            if (!projectHeadlineCheck) this.FIELD_PROJECTHEADLINE_ERROR = true;
            if (!projectPitchCheck) this.FIELD_PROJECTPITCH_ERROR = true;
            passed = false;
        }

        if (this.selectedSemester === ESemester.None) {
            this.FIELD_SEMESTER_ERROR = true;
            passed = false;
        }

        if (this.projectUserList.length === 0) {
            this.FIELD_SEARCHBARCONTENT_ERROR = true;
            passed = false;
        }

        if (passed) {
            const idList: number[] = [];
            this.projectUserList.forEach( (user) => { idList.push(user.userId); });
            const payload: IProjectCreationViewModel = {
                actorId: this.authService.authenticationInfo.user.userId,
                semesterId: this.selectedSemester,
                name: this.projectName,
                headline: this.projectHeadline,
                pitch: this.projectPitch,
                members: idList
            };

            const response = await API.post(`${Endpoint.Project}/create`, payload);
            if (response.data) {
                this.closeProjectSetupWindow();
                this.fetchUserProjects(response.data);
            }
        }
    }
}
</script>

<style lang="scss">
.nav {
    position: absolute;
    top: 15px;
    left: 160px;
}

.custom-margin {
    margin-top: 5px;
}

.invisible {
    opacity: 0.5;
    user-select: none;
    pointer-events: none;
}

.cross {
    position: absolute;
    top: 30px;
    right: 50px;
    font-size: 180%;
    color: #919191;
    user-select: none;
    cursor: pointer;
}

.create-after {
    position: absolute;
    top: 70px;
    right: 55px;
}

.cover-photo {
    position: absolute;
    top: 160px;
    left: 0px;
    width: 846px;
    height: 247px;
    background-image: url("../assets/images/projects-cover.png");
}

.content {
    position: absolute;
    top: 420px;
}

.pretitle {
    color: #808080;
}

.project-title {
    font-family: "gotham-bold";
    font-size: 250%;
    margin-top: -20px;
}

.member {
    display: inline;
    margin-right: 5px;
}

.subtitle {
    font-size: 120%;
    color: #707070;
}

.searchResult {
    padding: 15px;
    border-radius: 25px;
    font-size: 130%;
    cursor: pointer;

    transition-property: box-shadow;
    transition-duration: 0.2s;
}

.searchResult:hover {
    box-shadow: 0px 0px 25px #e6e6e6;
}

.avatar {
    vertical-align: middle;
    border-radius: 50%;
}

.member-avatar {
    width: 50px;
    height: 50px;
    border-radius: 50%;
    border: 3px solid #e2e2e2;
}

.member-avatar.chief {
    border: 3px solid #ffbd09;
}

.add-button {
    display: inline;
    position: absolute;
    right: 120px;
    padding: 12px;
    font-family: "gotham-bold";
    color: black;
}

.add-member-button {
    cursor: pointer;
    transition-property: box-shadow;
    transition-duration: 0.2s;
}

.add-member-button:hover {
    box-shadow: 0px 0px 10px #8f8f8f;
}

.minititle {
    font-size: 110%;
    color: #919191;
}

.headline-content {
    font-size: 140%;
}

.special {
    position: absolute;
    bottom: 75px;
    width: 81.5%;
}

.productname {
    color: #4b80ac;
}

.action-buttons {
    position: relative;
    bottom: -40px;
    left: 220px;
}

.bottom-button {
    outline-width: 0;
    width: 133px;
    height: 133px;
    margin-right: 20px;
    background-color: white;
    border: none;
    cursor: pointer;
}

.inventory {
    background-image: url("../assets/images/button-inventory.png");
    
    transition-property: background-image;
    transition-duration: 0.2s;
}

.hidden {
    display: none;
}

.inventorysidetext {
    position: absolute;
    bottom: 45px;
    left: -230px;
    width: 200px;
    height: 68px;
    background-image: url("../assets/images/client-pleasure.png");
}

.accountssidetext {
    position: absolute;
    bottom: 55px;
    left: 310px;
    width: 199px;
    height: 52px;
    background-image: url("../assets/images/who-own-you.png");
}

.inventory:hover {
    background-image: url("../assets/images/button-inventory-hovered.png");
}

.inventoryselected {
    background-image: url("../assets/images/button-inventory-hovered.png");
}

.accounts {
    background-image: url("../assets/images/button-accounts.png");
    
    transition-property: background-image;
    transition-duration: 0.2s;
}

.accounts:hover {
    background-image: url("../assets/images/button-accounts-hovered.png");
}

.accountsselected {
    background-image: url("../assets/images/button-accounts-hovered.png");
}

.info-account-title {
    font-size: 140%;
    color: #888888;
}

.badBalance {
    color: #d12027;
}

.goodBalance {
    color: #4eb748;
}
</style>