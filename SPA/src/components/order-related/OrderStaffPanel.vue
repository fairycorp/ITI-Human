<template>
    <div class="main">
        <div class="under-bar">
            <a href="#" @click="viewAllProjects()">Tous les projets</a>
        </div>
        
        <div class="content">
            <div v-if="viewProjectAll" class="projects">
                <el-table :data="renderData">
                    <el-table-column
                        prop="semesterName"
                        label="Semestre">
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
                        label="Inventaire N°">
                    </el-table-column>
                </el-table>
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
export default class OrderStaffPanel extends Vue {
    private tableData!: any[];
    private viewOrderAll: boolean = false;
    private viewOrderByUser: boolean = false;
    private viewOrderFromProject: boolean = false;
    private viewProjectAll: boolean = false;

    /** Gets Project list. */
    public get renderData() { return this.tableData; }

    constructor() {
        super();
    }

    private async getAllProjects() {
        API.get(Endpoint.Project).then( (response) => {
            this.tableData = response.data;
            this.viewProjectAll = true;
        });
    }

    private viewAllProjects() {
        this.getAllProjects();
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
</style>
