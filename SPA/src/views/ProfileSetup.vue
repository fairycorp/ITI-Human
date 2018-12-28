<template>
    <div class="global-content">
        <div 
            class="left-page">
            <h1>Setup de votre profil</h1>

            <h3 class="medium-top-margin">MINUSCULE ETAT CIVIL</h3>
            <input
                v-model="frstname"
                class="textual light-top-margin"
                :class="{ error : FIELD_FRSTNAME_ERROR }"
                placeholder="Votre prénom" />
            <input
                v-model="lastname"
                :class="{ error : FIELD_LASTNAME_ERROR }"
                class="textual medium-top-margin"
                placeholder="Votre nom de famille" />
            <input
                v-model="biography"
                class="textual medium-top-margin"
                placeholder="Votre (courte) bio" />

            <h3 class="high-top-margin">RELATIF À L'ÉCOLE</h3>
            <div class="choice">
                <div 
                    @click="displaySection(3)"
                    :class="{ selectedSection: SECTION_INTECH }"
                    class="category intech unselectable-text">
                    ITI
                </div>
                <div
                    @click="displaySection(1)"
                    :class="{ selectedSection: SECTION_TEACH }"
                    class="category teach unselectable-text">
                    PROFESSEUR
                </div>
                <div
                    @click="displaySection(2)"
                    :class="{ selectedSection: SECTION_ADMIN }"
                    class="category admin unselectable-text">
                    MEMBRE DE L'ADMINISTRATION
                </div>
            </div>
            <div class="sections">
                <div
                    v-if="!SECTION_INTECH && !SECTION_TEACH && !SECTION_ADMIN"
                    class="no-selected-section-text medium-top-margin">
                    Choisissez l'une des trois options ci-dessus pour continuer.
                </div>
                <div v-if="SECTION_INTECH" class="section medium-top-margin">
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
                        class="selection light-right-margin">S4</button>
                        <button
                        @click="selectSemester(5)"
                        :class="{ selectedButton: selectedSemester === 5, error: FIELD_SEMESTER_ERROR }"
                        class="selection light-right-margin">S5</button>
                        <br /><br />
                        <button
                        @click="selectSemester(6)"
                        :class="{ selectedButton: selectedSemester === 6, error: FIELD_SEMESTER_ERROR }"
                        class="selection light-right-margin">S6</button>
                        <button
                        @click="selectSemester(7)"
                        :class="{ selectedButton: selectedSemester === 7, error: FIELD_SEMESTER_ERROR }"
                        class="selection light-right-margin">S7</button>
                        <button
                        @click="selectSemester(8)"
                        :class="{ selectedButton: selectedSemester === 8, error: FIELD_SEMESTER_ERROR }"
                        class="selection light-right-margin">S8</button>
                        <button
                        @click="selectSemester(9)"
                        :class="{ selectedButton: selectedSemester === 9, error: FIELD_SEMESTER_ERROR }"
                        class="selection light-right-margin">S9</button>
                        <button
                        @click="selectSemester(10)"
                        :class="{ selectedButton: selectedSemester === 10, error: FIELD_SEMESTER_ERROR }"
                        class="selection light-right-margin">S10</button>
                </div>
                <div v-if="SECTION_TEACH" class="section medium-top-margin">
                    <span class="infotext">
                        L’obtention du statut de professeur nécessite l’usage d’un code secret.
                    </span>
                    <input
                        v-model="secretCode"
                        class="textual"
                        placeholder="Insérez ici le code secret" />
                </div>
                <div v-if="SECTION_ADMIN" class="section medium-top-margin">
                    <span class="infotext">
                        L’obtention du statut de membre de l'administration nécessite l’usage d’un code secret.
                    </span>
                    <input
                        v-model="secretCode"
                        class="textual"
                        placeholder="Insérez ici le code secret" />
                </div>
            </div>
            <div
                v-if="SECTION_INTECH || SECTION_TEACH || SECTION_ADMIN"
                class="take-note-text high-top-margin">
                <span class="openSans-bold">Merci de prendre note :</span>
                <br />
                Qu’en validant ce formulaire, vous attestez que l’ensemble des informations y figurant sont à la fois véridiques,
                et vérifiables, et que si cette clause n’est pas respectée, votre compte est susceptible d’être désactivé.
            </div>
            <div
                v-if="SECTION_INTECH || SECTION_TEACH || SECTION_ADMIN"
                :class="{ intechButtonMargin : SECTION_INTECH, teachButtonMargin : SECTION_TEACH, adminButtonMargin : SECTION_ADMIN}"
                class="submit-button">
                <div
                    @click="submit()"
                    class="submit-button-text unselectable-text">
                    VALIDEZ-MOI.
                </div>
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

import { ESStatus } from "@/models/model.SchoolStatus";
import { ESemester } from "@/models/model.Semester";
import { IStudentProfileSetup } from "@/models/model.User";

@Component({})
export default class ProfileSetup extends Vue {
    @Prop() private authService!: AuthService;
    private frstname!: string;
    private lastname!: string;
    private biography!: string;
    private secretCode!: string;
    private selectedSemester: number = 0;
    private FIELD_SEMESTER_ERROR: boolean = false;
    private FIELD_FRSTNAME_ERROR: boolean = false;
    private FIELD_LASTNAME_ERROR: boolean = false;
    private SECTION_INTECH: boolean = false;
    private SECTION_ESIEA: boolean = false;
    private SECTION_TEACH: boolean = false;
    private SECTION_ADMIN: boolean = false;

    constructor() {
        super();
        this.isAccessible();
    }
    /** Watches authService instance information. */
    @Watch("authService.authenticationInfo.level")
    private onAuthLevelChange() {
        if (this.authService.authenticationInfo.level === 0) {
            Vue.prototype.$authService = this.authService;
            this.$router.push("/");
        }
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

    /** Checks argument admissibility.
     * @param inputData Input data to check.
     * @param length Wanted inputData minimum length.
     */
    private inputDataCheck(inputData: string, length: number): boolean {
        if (inputData !== null && inputData !== undefined) {
            if (inputData.length < length || inputData.includes(" ")) {
                return false;
            }
            return true;
        }
        return false;
    }

    // SECTION TOGGLING METHODS.
    /** Displays matching section according to user's choice. */
    private displaySection(schoolStatus: number) {
        this.selectedSemester = ESemester.None;
        this.secretCode = "";

        const newStatus = schoolStatus as ESStatus;
        switch (newStatus) {
            case (ESStatus.None):
                this.SECTION_INTECH = false;
                this.SECTION_ESIEA = false;
                this.SECTION_TEACH = false;
                this.SECTION_ADMIN = false;
                break;

            case (ESStatus.Student):
                this.SECTION_INTECH = true;
                this.SECTION_ESIEA = false;
                this.SECTION_TEACH = false;
                this.SECTION_ADMIN = false;
                break;

            case (ESStatus.Teacher):
                this.SECTION_INTECH = false;
                this.SECTION_ESIEA = false;
                this.SECTION_TEACH = true;
                this.SECTION_ADMIN = false;
                break;

            case (ESStatus.Administration):
                this.SECTION_INTECH = false;
                this.SECTION_ESIEA = false;
                this.SECTION_TEACH = false;
                this.SECTION_ADMIN = true;
                break;
        }
        return;
    }

    /** Selects semester according to user's selection. */
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

            case (ESemester.S5):
                this.selectedSemester = ESemester.S5 as number;
                this.FIELD_SEMESTER_ERROR = false;
                break;

            case (ESemester.S6):
                this.selectedSemester = ESemester.S6 as number;
                this.FIELD_SEMESTER_ERROR = false;
                break;

            case (ESemester.S7):
                this.selectedSemester = ESemester.S7 as number;
                this.FIELD_SEMESTER_ERROR = false;
                break;

            case (ESemester.S8):
                this.selectedSemester = ESemester.S8 as number;
                this.FIELD_SEMESTER_ERROR = false;
                break;

            case (ESemester.S9):
                this.selectedSemester = ESemester.S9 as number;
                this.FIELD_SEMESTER_ERROR = false;
                break;

            case (ESemester.S10):
                this.selectedSemester = ESemester.S10 as number;
                this.FIELD_SEMESTER_ERROR = false;
                break;
        }
    }

    // SUBMIT METHODS.
    /** Submits main form. */
    private async submit() {
        const frstnameCheck = this.inputDataCheck(this.frstname, 2);
        const lastnameCheck = this.inputDataCheck(this.lastname, 2);
        if (!frstnameCheck || !lastnameCheck) {
            if (!frstnameCheck) this.FIELD_FRSTNAME_ERROR = true;
            if (!lastnameCheck) this.FIELD_LASTNAME_ERROR = true;
        }

        if (this.SECTION_INTECH) {
            if (this.selectedSemester === ESemester.None) {
                this.FIELD_SEMESTER_ERROR = true;
                return;
            }

            const payload: IStudentProfileSetup = {
                userId: this.authService.authenticationInfo.user.userId,
                firstname: this.frstname,
                lastname: this.lastname,
                desc: this.biography,
                schoolStatusId: ESStatus.Student as number,
                semesterId: this.selectedSemester
            };

            const response = await API.post(`${Endpoint.User}/setup`, payload);
            if (response.status === 200) this.$router.push("/landing");

        } else if (this.SECTION_TEACH) {
        } else if (this.SECTION_ADMIN) {
        } else {
        }
    }
}
</script>

<style lang="scss">
.category {
    font-family: "gotham-bold";
    font-size: 80%;
    letter-spacing: 2px;
    color: #c2c2c1;
    cursor: pointer;

    transition-property: color;
    transition-duration: 0.2s;
}
.category:hover, .selectedSection{
    color: #808080;
}

.category.intech {
    position: absolute;
    bottom: 49%;
    width: 3%;
}

.category.teach {
    position: absolute;
    left: 110px;
    bottom: 49%;
    width: 17%;
}

.category.admin {
    position: absolute;
    left: 240px;
    bottom: 49%;
    width: 40%;
}

.no-selected-section-text {
    font-family: "OpenSans-italic";
    font-size: 80%;
    color: #bdbdbd;
}

.infotext {
    color: #b86565;
}

.take-note-text {
    font-size: 80%;
    text-align: justify;
}

.intechButtonMargin {
    margin-top: 130px;
}

.teachButtonMargin, .adminButtonMargin {
    margin-top: 186px;
}
</style>
