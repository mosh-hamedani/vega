import { CanActivate } from '@angular/router';
import { Auth } from './auth.service';
import { Injectable } from '@angular/core';

@Injectable()
export class AuthGuard implements CanActivate {

  constructor(protected auth: Auth) { }

  canActivate() { 
    if (this.auth.authenticated())
      return true;

    window.location.href = 'https://vegaproject.auth0.com/login?client=RfRu3un13aOO73C7X2mH41qxfHRbUc33';
    return false;
  }
}