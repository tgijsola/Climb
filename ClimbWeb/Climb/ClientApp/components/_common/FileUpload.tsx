import { ClimbClient } from "../../gen/climbClient";

export class FileUpload implements ClimbClient.FileParameter {
    data: any;
    fileName: string;

    constructor(file: File) {
        this.data = file;
        this.fileName = file.name;
    }
}