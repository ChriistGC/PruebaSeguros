import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment.development';
import { Cliente, ClienteSeguro, Seguro } from '../interface/seguros.interface';

@Injectable({
  providedIn: 'root'
})
export class SeguroService {

  private baseUrl: string = environment.baseUrl;

  constructor(private http: HttpClient) { }

  getClientes(): Observable<Cliente[]> {
    return this.http.get<Cliente[]>(`${this.baseUrl}/cliente`)
  }

  getSeguros() {
    return this.http.get<any[]>(`${this.baseUrl}/seguro`)
  }

  getCliente(cedula: string): Observable<any> {
    const params = new HttpParams()
      .set('q', cedula.toString());
    return this.http.get<Cliente>(`${this.baseUrl}/datos/seguros/${cedula}`);
  }
  getSeguro(codigo: string): Observable<any> {
    const params = new HttpParams()
      .set('q', codigo.toString());
    return this.http.get<Seguro>(`${this.baseUrl}/datos/clientes/${codigo}`);
  }
  
  postCliente(cliente: Cliente): Observable<any> {
    return this.http.post(`${this.baseUrl}/cliente`, cliente);
  }

  postSeguro(seguro: Seguro): Observable<any> {
    return this.http.post(`${this.baseUrl}/seguro`, seguro);
  }

  postFile(file: File): Observable<any> {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post(`${this.baseUrl}/cliente/file`, formData);
  }

  postAsignar(idCliente: ClienteSeguro): Observable<any> {
    return this.http.post(`${this.baseUrl}/datos`, idCliente);
  }

  putUsuario(id:number, cliente:Cliente):Observable<any>{
    return  this.http.put(`${this.baseUrl}/cliente/${id}`, cliente );
  }

  putSeguro(id:number, seguro:Seguro):Observable<any>{
    return this.http.put(`${this.baseUrl}/seguro/${id}`, seguro);
  }

  deleteCliente(id:number):Observable<any>{
    return this.http.delete(`${this.baseUrl}/cliente/${id}`);
  }

  deleteSeguro(id:number):Observable<any>{
    return this.http.delete(`${this.baseUrl}/seguro/${id}`);
  }

}
