<template>
    <div class="global-content">
        <div
            v-if="!WINDOW_ABOUT"
            class="default-logo">
        </div>
        <div
            v-else
            class="moved-logo">
        </div>
        <div
            v-if="WINDOW_ABOUT" 
            class="light-left-page">
            <h1>À propos</h1>

            <h3 class="light-top-margin lightly-reduced-bottom-margin">
                QUI ÊTES-VOUS VRAIMENT ?
            </h3>
            <p class="openSans-litalic">
                Rien de plus que de simples étudiants, à vrai dire ! <span class="openSans-bold">Sylvain Goffart</span>
                et <span class="openSans-bold">Pierre Viara</span> pour être exact.
            </p>

            <h3 class="medium-top-margin lightly-reduced-bottom-margin">
                QU'EST-CE QUE FORK ?
            </h3>
            <p class="openSans-litalic">
                En fait, au cours du quatrième semestre à <span class="openSans-bold">IN’TECH</span>, les étudiants sont 
                chargés d’organiser un événement, qu’ils doivent eux-mêmes financer. Pour se faire, ils organisent en
                équipe ce qu’on appelle des “buvettes.”<br />
                Le but de <span class="openSans-bold">Fork</span> c’est tout simplement de <span class="openSans-bold">digitaliser les échanges</span>
                entre ces <span class="openSans-bold">équipes</span> et leurs <span class="openSans-bold">clients</span>.
            </p>

            <h3 class="medium-top-margin lightly-reduced-bottom-margin">
                J'AI ENCORE PLEIN DE QUESTIONS.
            </h3>
            <p class="openSans-litalic">
                À la bonne heure, les amis ! Nous serions absolument ravis d’y répondre, si toutefois vous nous contactez.
                Et cela tombe très bien, puisque nous avons une adresse électronique <span class="openSans-bold">à votre
                entière disposition</span>. Oui.
            </p>
            <a href="mailto:fork@intechinfo.fr">
                <div class="contact-button medium-top-margin"></div>
            </a>
        </div>
        <div 
            class="right-page">
            <h1>Envie d'un cookie ?</h1><div class="cookie-image"></div>
            <div class="choice">
                <button @click="startGithub()" class="standard light-top-margin light-right-margin">OOOH, OUI !</button>
                <button class="standard light-top-margin">EUH, NON.</button>
                <div class="nota-bene">
                    * Alors là, attention parce que le <span class="openSans-bold">bouton de gauche</span> lance une connexion avec Github.
                </div>
            </div>
            <div class="cookies-cover-image"></div>

            <h2 class="high-top-margin">BUREAU DU STAFF</h2>
            <input
                v-model="username"
                :class="{ error : FIELD_USERNAME_ERROR }" 
                class="light-top-margin textual" 
                type="text"
                placeholder="Votre petit nom"
            />
            <input
                :class="{ error : FIELD_PASSWORD_ERROR }" 
                v-model="password"
                class="textual medium-top-margin"
                type="password"
                placeholder="Votre mot de passe" 
            />
            <div class="submit-button high-top-margin">
                <div
                    @click="startBasic(username, password)"
                    class="submit-button-text unselectable-text">
                    CONNECTEZ-MOI.
                </div>
            </div>

            <div class="footer">
                <div
                    @click="toggleWindowAbout"
                    class="about-link unselectable-text"
                    >QUI SOMMES-NOUS
                </div>
                <div class="collab-info">EN COLLABORATION AVEC IN'TECH</div>
            </div>
        </div>
    </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";
import { appSettings } from "@/config/appSettings";
import {
  AuthService,
  IAuthServiceConfiguration
} from "@signature/webfrontauth";
import Axios from "axios";

@Component({})
export default class Homepage extends Vue {
    private authService!: AuthService;
    private authSerConf!: IAuthServiceConfiguration;
    private username: string = "";
    private password: string = "";
    private FIELD_USERNAME_ERROR: boolean = false;
    private FIELD_PASSWORD_ERROR: boolean = false;
    private WINDOW_ABOUT: boolean = false;

    constructor() {
        super();
        this.authSerConf = appSettings;
        this.authService = new AuthService(this.authSerConf, Axios);
        this.authService.refresh(true, true, true);
    }
    // DATACHECKING METHODS.
    /** Checks argument admissibility.
     * @param inputData Input data to check.
     */
    private inputDataCheck(inputData: string): boolean {
        if (inputData !== null && inputData !== undefined) {
            if (inputData.length < 4 || inputData.includes(" ")) {
                return false;
            }
            return true;
        }
        return false;
    }
    // AUTHENTICATION METHODS.
    /** Launches Github authentication process
     * by opening its matching popup.
     */
    private async startGithub() {
        return await this.authService.startPopupLogin("GitHub");
    }

    /** Launches basic authentication process
     * by retrieving inputs data (login/psswd).
     */
    private async startBasic(username: string, password: string) {
        const usernameCheck = this.inputDataCheck(username);
        const passwordCheck = this.inputDataCheck(password);
        if (!usernameCheck || !passwordCheck) {
            if (!usernameCheck) this.FIELD_USERNAME_ERROR = true;
            if (!passwordCheck) this.FIELD_PASSWORD_ERROR = true;
            return;
        }
        // Resets potential previous errors.
        this.FIELD_USERNAME_ERROR = this.FIELD_PASSWORD_ERROR
            = false;

        // TODO: Start basic authentication...
    }
    // WINDOW TOGGLING METHODS.
    /** Displays or undisplays "About" window. */
    private toggleWindowAbout() {
        return this.WINDOW_ABOUT = !this.WINDOW_ABOUT;
    }
}
</script>

<style lang="scss">
.default-logo {
    position: absolute;
    top: 35%;
    left: 21%;
    width: 252px;
    height: 270px;
    background-image: url("../assets/images/logo.png");
}

.moved-logo {
    position: absolute;
    top: 65%;
    left: 21%;
    width: 252px;
    height: 270px;
    background-image: url("../assets/images/logo.png");
}

.cookie-image {
    position: absolute;
    top: 75px;
    left: 360px;
    width: 34px;
    height: 33px;
    background-image: url("../assets/images/cookie.png");
}

.nota-bene {
    margin-top: 15px;
    font-size: 70%;
}

.cookies-cover-image {
    position: relative;
    top: 30px;
    left: -70px;
    width: 847px;
    height: 303px;
    background-image: url("../assets/images/cookies.png");
}

.footer {
    position: absolute;
    bottom: 75px;
    font-size: 14px;
}

.about-link {
    position: absolute;
    left: 0;
    width: 220px;
    font-family: "gotham-bold";
    letter-spacing: 3px;
    color: #666567;
    cursor: pointer;
}

.collab-info {
    position: absolute;
    left: 360px;
    width: 600px;
    font-family: "gotham-bold";
    letter-spacing: 3px;
    color: #cac8c9;
}

.contact-button {
    width: 193px;
    height: 46px;
    background-image: url("../assets/images/contact-button.png");
    cursor: pointer;
}

.specific-input-error-icon {
    width: 15px;
    height: 15px;
    background-image: url("../assets/images/input-error.png");
}
</style>
