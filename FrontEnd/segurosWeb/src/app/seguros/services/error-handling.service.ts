import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ErrorHandlingService {

  constructor() { }

  formatError(error: HttpErrorResponse): string {
    if (error.status >= 400 && error.status < 500) {
      const clientMessage = this.getFormattedError(error.error?.errores?.Cliente, 'Cliente');
      const seguroMessage = this.getFormattedError(error.error?.errores?.Seguro, 'Seguro');
      const archivoMessage = this.getFormattedError(error.error?.errores?.Archivo, 'Archivo');
      const ClienteSeguroMessage = this.getFormattedError(error.error?.errores?.ClienteSeguro, 'ClienteSeguro');
      const combinedMessages = [clientMessage, seguroMessage, archivoMessage, ClienteSeguroMessage].filter(msg => !!msg).join('\n\n');
      
      return combinedMessages || 'Error desconocido';
    } else if (error.status >= 500 && error.status < 600) {
      console.log(error.message)
      return `Error del servidor`;
    } else {
      return `Error en la solicitud`;
    }
  }

  private getFormattedError(error: any, type: string): string {
    if (error && Array.isArray(error)) {
      return this.getFormattedErrors(error, type);
    } else if (error) {
      return `${type}:\n${error}`;
    }
    return '';
  }
  
  private getFormattedErrors(errors: any[], type: string): string {
    if (!errors || !Array.isArray(errors)) {
      return '';
    }
    const errorMessages = errors.map((e: any) => e.ErrorMessage).join('\n');
    return `${type}:\n${errorMessages}`;
  }
}
