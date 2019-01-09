interface IBasicDataProject {
    projectId: number;
    projectTypeId: number;
    projectTypeName: string;
    projectName: string;
    projectHeadline: string;
    projectPitch: string;
    semesterId: number;
    semesterName: string;
    storageId: number;
    members: IBasicDataProjectMember[];
}

interface IBasicDataProjectMember {
    projectMemberId: number;
    projectId: number;
    projectRankId: number;
    projectRankName: string;
    userId: number;
    userName: string;
}

interface ProjectCreationViewModel {
    actorId: number;
    semesterId: number;
    name: string;
    headline: string;
    pitch: string;
    members: number[];
}

export { IBasicDataProject, ProjectCreationViewModel };
