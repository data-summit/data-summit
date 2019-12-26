import { Injectable } from "@angular/core";
import { IndustryType } from "../../enums/industry-type.enum";
import { IndustryVariables } from "../models/industry-variables";

@Injectable()
export class IndustryVariablesService {
    
    industry: number;
    industryIcon: string;
    buttonColour: string;
    industryText: string;

    /**Takes the industry passed into the routeParams and sets the icon and header text */
    setIndustry(ind: IndustryType) {
        switch (ind) {
          case IndustryType.unknown:
            this.industryIcon = "fa-question";
            this.buttonColour = "";
            this.industryText = "";
            break;
          case IndustryType.construction:
            this.industryIcon = "fa-building cyan-text";
            this.buttonColour = "cyan";
            this.industryText = "Construction";
            break;
          case IndustryType.finance:
            this.industryIcon = "fa-hand-holding-usd orange-text";
            this.buttonColour = "orange";
            this.industryText = "Finance";
            break;
          case IndustryType.energy:
            this.industryIcon = "fa fa-power-off red-text";
            this.buttonColour = "red";
            this.industryText = "Energy";
            break;
          case IndustryType.healthcare:
            this.industryIcon = "fas fa-clinic-medical blue-text";
            this.buttonColour = "blue";
            this.industryText = "Healthcare";
            break;
          case IndustryType.pharmaceutical:
            this.industryIcon = "fa fa-capsules DarkGoldenrod-text";
            this.buttonColour = "DarkGoldenrod";
            this.industryText = "Pharmaceutical";
            break;
          case IndustryType.government:
            this.industryIcon = "fas fa-award black-text";
            this.buttonColour = "black";
            this.industryText = "Government";
            break;
          case IndustryType.legal:
            this.industryIcon = "fas fa-balance-scale darkblue-text";
            this.buttonColour = "DarkBlue";
            this.industryText = "Legal";
            break;
          case IndustryType.manufacturing:
            this.industryIcon = "fas fa-industry brown-text";
            this.buttonColour = "brown";
            this.industryText = "Manufacturing";
            break;
          case IndustryType.IT:
            this.industryIcon = "fa fa-desktop mediumSeaGreen-text";
            this.buttonColour = "green";
            this.industryText = "IT";
            break;
          default:
            this.industryIcon = "fa-building cyan-text";
            this.buttonColour = "cyan"
            this.industryText = "Construction"
            break;
        }
      }

      getIndustryInfo(): IndustryVariables {
          return {
              colour: this.buttonColour,
              text: this.industryText,
              icon: this.industryIcon
          }
      }
}