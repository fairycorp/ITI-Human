<template>
    <div class="main">
        <div class="under-bar">
            <a href="#" @click="getAllProjects()">Tous les projets</a>
            - <a href="#" @click="getAllOrders()">Toutes les commandes</a>
        </div>
        
        <div class="content">
            <div v-if="viewProjectAll" class="projects">
                <el-table :data="tableData">
                    <el-table-column
                        prop="semesterName"
                        label="Semestre"
                        width="80">
                    </el-table-column>
                    <el-table-column
                        prop="projectTypeName"
                        label="Type de Projet">
                    </el-table-column>
                    <el-table-column
                        prop="projectName"
                        label="Nom du projet">
                    </el-table-column>
                    <el-table-column
                        prop="projectHeadline"
                        label="Slogan">
                    </el-table-column>
                    <el-table-column
                        prop="projectPitch"
                        label="Pitch">
                    </el-table-column>
                    <el-table-column
                        prop="storageId"
                        label="Inventaire N°"
                        width="110">
                    </el-table-column>
                </el-table>
            </div>
            <div v-else-if="viewOrderAll">
                <el-table :data="tableData">
                    <el-table-column
                        prop="info.orderId"
                        label="Identifiant"
                        width="110">
                    </el-table-column>
                    <el-table-column
                        prop="info.userName"
                        label="Nom d'utilisateur">
                    </el-table-column>
                    <el-table-column
                        prop="info.creationDate"
                        label="Date de création">
                    </el-table-column>
                    <el-table-column
                        prop="info.classroomName"
                        label="Livraison">
                    </el-table-column>
                    <el-table-column
                        prop="info.total"
                        label="Total">
                    </el-table-column>
                    <el-table-column
                        prop="info.currentState"
                        label="Etat">
                    </el-table-column>
                </el-table>
            </div>
            <div v-else class="none">
                Aucune option d'affichage sélectionnée.
            </div>
        </div>
    </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from "vue-property-decorator";
import { IProject } from "@/model/order/OrderModel";
import API from "@/services/API";
import Endpoint from "../../helpers/Endpoint";

@Component
export default class StaffDashboard extends Vue {
    public tableData!: any[];
    private viewOrderAll: boolean = false;
    private viewOrderByUser: boolean = false;
    private viewOrderFromProject: boolean = false;
    private viewProjectAll: boolean = false;

    constructor() {
        super();
        this.getAllProjects();
    }

    private async getAllProjects() {
        API.get(Endpoint.Project).then( (response) => {
            this.tableData = response.data;

            this.viewOrderByUser = false;
            this.viewOrderFromProject = false;
            this.viewProjectAll = true;
            this.viewOrderAll = false;
        });
    }

    private async getAllOrders() {
        API.get(Endpoint.Order).then( (response) => {
            this.tableData = response.data;

            this.viewOrderByUser = false;
            this.viewOrderFromProject = false;
            this.viewProjectAll = false;
            this.viewOrderAll = true;
        });
    }
}
</script>

<style>
    .under-bar {
        height: 30px;
        padding-top: 5px;
        padding-left: 15px;
        background-color: #01b04b;
    }

    .under-bar > a {
        color: white;
    }

    .none {
        margin-top: 100px;
        text-align: center;
        font-size: 180%;
        color: grey;
    }

    .financial {
        background-color: grey;
        border-radius: 5px;
        color: white;
        cursor: pointer;
    }
</style>
