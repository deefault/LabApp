import {EventEmitter, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {map} from 'rxjs/operators';
import {Router} from '@angular/router';
import {HttpService} from "../http/http.service";
import {UserProfile} from "../../models/interfaces/user-profile";
import {JwtHelperService} from "@auth0/angular-jwt";
import {UserType} from "../../enums/user-type.enum";
import {AuthenticationService} from "../../clients/common";


export interface JwtClaims {
  Id: number;
  Role: UserType;
  Email: string;
}


@Injectable({
  providedIn: 'root'
})
export class AuthService {
  static TokenName: string = 'authToken';

  public isLoggedIn: boolean;
  public user: UserProfile;
  public userIdentity: JwtClaims;
  public logged: EventEmitter<any> = new EventEmitter<any>();

  private jwtHelper: JwtHelperService;


  constructor(private http: HttpClient, private router: Router, private httpService: HttpService,
              private authenticationApiService: AuthenticationService
  ) {
    this.isLoggedIn = localStorage.getItem(AuthService.TokenName) != null;
    this.jwtHelper = new JwtHelperService();
    if (this.isLoggedIn) this.userIdentity = this.decodeToken();
  }

  get isTeacher(): boolean {
    return this.userIdentity.Role == UserType.Teacher;
  }

  login(username: string, password: string) {
    let data = new FormData();
    data.append("username", username);
    data.append("password", password);
    //return this.http.post(this.httpService.getUrl('M/authentication/token'), data, {responseType: "text"})
    return this.authenticationApiService.token(username, password, "response")
      .pipe(map(response => {
        localStorage.setItem(AuthService.TokenName, response.body as string);
        this.isLoggedIn = true;
        this.userIdentity = this.decodeToken();
        this.setUserDetails();
        this.logged.emit();
        return response;
      }));
  }

  private setUserDetails() {
    if (localStorage.getItem(AuthService.TokenName)) {

    }
  }

  logout() {
    this.userIdentity = null;
    this.isLoggedIn = false;
    localStorage.removeItem(AuthService.TokenName);
    this.router.navigate(['/login']);
  }

  private getToken(): string {
    return localStorage.getItem(AuthService.TokenName);
  }

  private decodeToken(): JwtClaims {
    let decoded = this.jwtHelper.decodeToken(this.getToken());
    let role: UserType = UserType[decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] as string];
    let claims: JwtClaims = new class implements JwtClaims {
      Email: string = decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"];
      Id: number = Number.parseInt(decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"]);
      Role: UserType = role;
    }

    return claims
  }
}
