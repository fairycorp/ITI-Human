<template>
    <div class="main">
        <div class="left-page" :class="{ invisible : WINDOW_PROJECT_SETUP || WINDOW_PROJECT_ADD_MEMBER }">
            <h1>Liste de vos projets</h1>
            <div class="cover-photo"></div>

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
                </div>
                <div v-else>
                    <div class="project-title">Absolument rien.</div>
                    <button @click="launchProjectSetup()" class="light-top-margin standard">CREER UN PROJET</button>
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
                    @click="submit()"
                    class="submit-button-text unselectable-text">
                    ALLEZ-Y LES GARS, CREEZ MON PROJET !
                </div>
            </div>
        </div>
        <div v-if="displayedProject !== null && displayedProject !== undefined && WINDOW_PROJECT_ADD_MEMBER" class="right-page">
            <div @click="closeProjectAddMemberWindow()" class="cross">x</div>
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

import { IBasicDataProject, ProjectCreationViewModel, IBasicDataProjectMember } from "@/models/model.Project";
import { ESStatus } from "@/models/model.SchoolStatus";
import { ESemester } from "@/models/model.Semester";
import { IDetailedDataUser } from "@/models/model.User";

@Component({})
export default class ProjectsDB extends Vue {
    @Prop() private authService!: AuthService;
    @Prop() private displayedProject!: IBasicDataProject;
    @Prop() private currentUserSchoolProfile!: IBasicDataProjectMember;
    private userProjects: IBasicDataProject[] = [];
    private userList: IDetailedDataUser[] = [];
    private projectName: string = "";
    private projectHeadline: string = "";
    private projectPitch: string = "";
    private selectedSemester: number = 0;
    private searchBarContent: string = "";
    private searchResult: IDetailedDataUser | null = null;
    private projectUserList: IDetailedDataUser[] = [];
    private WINDOW_PROJECT_SETUP: boolean = false;
    private WINDOW_PROJECT_ADD_MEMBER: boolean = false;
    private FIELD_PROJECTNAME_ERROR: boolean = false;
    private FIELD_PROJECTHEADLINE_ERROR: boolean = false;
    private FIELD_PROJECTPITCH_ERROR: boolean = false;
    private FIELD_SEMESTER_ERROR: boolean = false;
    private FIELD_SEARCHBARCONTENT_ERROR: boolean = false;

    constructor() {
        super();
        this.isAccessible();
        this.fetchUserProjects();
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

    // GETTERS METHODS.
    private async fetchUserProjects() {
        const response = await API
            .get(`${Endpoint.Project}/u/${this.authService.authenticationInfo.user.userId}`);
        this.userProjects = response.data;

        this.displayedProject = this.userProjects[0];

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

    private changeCurrentDisplayedProject(newIndex: number) {
        for (let index: number = 0; index < this.userProjects.length; index++) {
            if (this.userProjects[index] === this.displayedProject) {
                this.displayedProject = this.userProjects[index + (newIndex)];
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

    private closeProjectSetupWindow() {
        this.WINDOW_PROJECT_SETUP = false;
    }

    private closeProjectAddMemberWindow() {
        this.WINDOW_PROJECT_ADD_MEMBER = false;
    }

    // SUBMITTING METHODS.
    private async submit() {
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
            const payload: ProjectCreationViewModel = {
                actorId: this.authService.authenticationInfo.user.userId,
                semesterId: this.selectedSemester,
                name: this.projectName,
                headline: this.projectHeadline,
                pitch: this.projectPitch,
                members: idList
            };

            const response = API.post(`${Endpoint.Project}/create`, payload)
            .then( (response) => {
                this.WINDOW_PROJECT_SETUP = false;
                this.fetchUserProjects();
            });
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
</style>