import { SaveVehicle } from './../models/vehicle';
import { Injectable } from '@angular/core';
import { Http } from '@angular/http'; 
import 'rxjs/add/operator/map';

@Injectable()
export class VehicleService {
  private readonly vehiclesEndpoint = '/api/vehicles';
  constructor(private http: Http) { }

  getFeatures() {
    return this.http.get('/api/features')
      .map(res => res.json());
  }

  getMakes() {
    return this.http.get('/api/makes')
      .map(res => res.json());
  }

  create(vehicle) {
    return this.http.post(this.vehiclesEndpoint, vehicle)
      .map(res => res.json());
  }

  getVehicle(id) {
    return this.http.get(this.vehiclesEndpoint + '/' + id)
      .map(res => res.json());
  }

  getVehicles(filter) {
    return this.http.get(this.vehiclesEndpoint + '?' + this.toQueryString(filter))
      .map(res => res.json());
  }

  toQueryString(obj) {
    var parts = [];
    for (var property in obj) {
      var value = obj[property];
      if (value != null && value != undefined) 
        parts.push(encodeURIComponent(property) + '=' + encodeURIComponent(value));
    }

    return parts.join('&');
  }

  update(vehicle: SaveVehicle) {
    return this.http.put(this.vehiclesEndpoint + '/' + vehicle.id, vehicle)
      .map(res => res.json());
  }

  delete(id) {
    return this.http.delete(this.vehiclesEndpoint + '/' + id)
      .map(res => res.json());
  }
}
