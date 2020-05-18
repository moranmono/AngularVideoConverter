import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpRequest, HttpEventType, HttpResponse } from '@angular/common/http';


@Component({
  selector: 'app-file-upload',
  templateUrl: './file-upload.component.html',
  styleUrls: ['./file-upload.component.css']
})
export class FileUploadComponent {
  public progress: number;
  public message: string;
  constructor(private http: HttpClient) { }

  fileUpload(files) {
    if (files.length === 0) {
      return;
    }
    const formData = new FormData();
    for (let file of files) {
      formData.append(file.name, file);
    }
    const uploadReq = new HttpRequest('POST', 'api/fileupload/file-upload', formData, {
      reportProgress: true,
    });

    this.http.request(uploadReq).subscribe(event => {
      if (event.type === HttpEventType.UploadProgress) {
        this.progress = Math.round(100 * event.loaded / event.total);
      }
      else if (event.type === HttpEventType.Response) {
        this.message = event.statusText;
      }
    });
  }

}
