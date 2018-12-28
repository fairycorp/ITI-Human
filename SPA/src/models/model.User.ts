import { ESemester } from "./model.Semester";
import { ESStatus } from "./model.SchoolStatus";

interface IUserBasicData {
    userId: number;
    userName: string;
    creationdate: Date;
}

interface IDetailedDataUser {
    userId: number;
    avatarUrl: string;
    userName: string;
    firstName: string;
    lastName: string;
    birthDate: Date;
    schoolMemberId: number;
    schoolStatusId: number;
    schoolStatusName: string;
}

interface IStandardProfileSetup {
    firstname: string;
    lastname: string;
    desc: string;
}

interface IStudentProfileSetup {
    userId: number;
    firstname: string;
    lastname: string;
    desc: string;
    schoolStatusId: ESStatus;
    semesterId: ESemester;
}

export { IUserBasicData, IDetailedDataUser, IStandardProfileSetup, IStudentProfileSetup };
