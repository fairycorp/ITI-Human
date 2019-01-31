<template>
    <f7-page>
        <h1>Setup de votre profil</h1>

        <h3 class="medium-top-margin">MINUSCULE ETAT CIVIL</h3>
        <f7-list class="listLogin" inline-labels no-hairlines-md>
          <f7-list-input class="textual light-top-margin" label="FirstName" :value="FirstName" @input="FirstName = $event.target.value" type="text" placeholder="Your FirstName">
          </f7-list-input>

          <f7-list-input  class="textual medium-top-margin" label="LastName" :value="LastName" @input="LastName = $event.target.value" type="text" placeholder="Your LastName">
          </f7-list-input>
        </f7-list> 


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
                    :class="{ error : FIELD_SECRETCODE_ERROR }"
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
    </f7-page>    
</template>

<script>
import Api from '../helpers/Api.js'
import {
  AuthService,
  IAuthServiceConfiguration,
  AuthLevel
} from "@signature/webfrontauth";
import {
    getAuthService
} from "../helpers/AuthServiceHelp.js";
import Axios from "axios";

export default {

}
</script>

<style>

</style>
