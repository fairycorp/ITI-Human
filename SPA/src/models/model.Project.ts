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
    openedStall: boolean;
    votes: number[];
    average: number;
}

interface IBasicDataProjectMember {
    projectMemberId: number;
    projectId: number;
    projectRankId: number;
    projectRankName: string;
    userId: number;
    userName: string;
    avatarUrl: string;
}

interface IProjectMemberCreationViewModel {
    projectId: number;
    userId: number;
}

interface IProjectCreationViewModel {
    actorId: number;
    semesterId: number;
    name: string;
    headline: string;
    pitch: string;
    members: number[];
}

export { IBasicDataProject, IProjectCreationViewModel, IBasicDataProjectMember, IProjectMemberCreationViewModel };
