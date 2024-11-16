import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MyConfig } from 'src/app/my-config';
import { ReportService, ReportData } from 'src/app/services/report.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent {
constructor(private http:HttpClient, private reportService: ReportService){}

public file:any
reportData: ReportData | undefined;

ngOnInit(): void {
  this.reportService.getReportData().subscribe(data => {
    this.reportData = data;
  });
}

generateReport() {
  this.http.get(MyConfig.APIurl + '/api/Report/GetPdfReport/pdf', { responseType: 'blob' })
    .subscribe((pdfBlob: Blob) => {
      const fileURL = URL.createObjectURL(pdfBlob);
      window.open(fileURL);
    }, error => {
      console.error('Error downloading the PDF:', error);
    });
}


}
