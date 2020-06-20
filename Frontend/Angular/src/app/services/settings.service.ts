import { Injectable } from '@angular/core';
import {NbThemeService} from "@nebular/theme";

@Injectable({
  providedIn: 'root'
})
export class SettingsService {

  private defaultTheme: string = 'default';


  themes: string[] = ['default', 'dark', 'cosmic']
  currentTheme: string;

  constructor(
    private themeService: NbThemeService,
  ) {
    this.currentTheme = localStorage.getItem("currentTheme");
    if (!this.currentTheme) this.currentTheme = this.defaultTheme
    this.changeTheme(this.currentTheme)
  }

  changeTheme(theme: string){
    this.currentTheme = theme;
    localStorage.setItem('currentTheme', this.currentTheme);
    this.themeService.changeTheme(theme);
  }
}
