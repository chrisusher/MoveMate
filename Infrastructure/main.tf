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

resource "random_integer" "ri" {
  min = 10000
  max = 99999
}

resource "azurerm_cosmosdb_account" "cosmosdb_account" {
  name                = "movemate-cosmos-db-${random_integer.ri.result}"
  location            = azurerm_resource_group.resource_group.location
  resource_group_name = azurerm_resource_group.resource_group.name
  offer_type          = "Standard"
  enable_free_tier    = true

  consistency_policy {
    consistency_level       = "Eventual"
    max_interval_in_seconds = 300
    max_staleness_prefix    = 100000
  }

  geo_location {
    location          = azurerm_resource_group.resource_group.location
    failover_priority = 0
  }

  capabilities {
    name = "EnableServerless"
  }
}

resource "azurerm_cosmosdb_sql_database" "movemate_prod_database" {
  name                = "movemate-prod"
  resource_group_name = azurerm_resource_group.resource_group.name
  account_name        = azurerm_cosmosdb_account.cosmosdb_account.name
}

resource "azurerm_cosmosdb_sql_database" "movemate_test_database" {
  name                = "movemate-test"
  resource_group_name = azurerm_resource_group.resource_group.name
  account_name        = azurerm_cosmosdb_account.cosmosdb_account.name
}

resource "azurerm_storage_account" "storageAccount" {
  name                              = "movematebc75"
  resource_group_name               = azurerm_resource_group.resource_group.name
  location                          = azurerm_resource_group.resource_group.location
  account_tier                      = "Standard"
  account_replication_type          = "LRS"
  account_kind                      = "Storage"
  default_to_oauth_authentication   = true
  cross_tenant_replication_enabled  = false
}

resource "azurerm_service_plan" "movemateServicePlan" {
  name                  = "ASP-movemate-b7fc"
  resource_group_name   = azurerm_resource_group.resource_group.name
  location              = azurerm_resource_group.resource_group.location
  os_type               = "Linux"
  sku_name              = "Y1"
}