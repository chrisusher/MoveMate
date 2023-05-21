terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~>3.0"
    }
  }
  backend "azurerm" {
      resource_group_name  = "cutestcloud-tstate-rg"
      storage_account_name = "cutestcloudtfstate20930"
      container_name       = "movemate-state"
      key                  = "terraform.tfstate"
  }
}

provider "azurerm" {
  features {}
}

resource "azurerm_resource_group" "resource_group" {
  name     = "movemate"
  location = "uksouth"
}