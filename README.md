# LeagueOfLegendsManager

## Descrição
Bem-vindo ao projeto LeagueOfLegendsManager, um Sistema de Gerenciamento de Itens e Campeões para o popular jogo League of Legends (LoL). Este projeto foi desenvolvido como um desafio para aprimorar habilidades em desenvolvimento .NET C# enquanto se trabalha em um contexto prático e divertido.

## Objetivo Geral
Desenvolver uma API RESTful em .NET C# que permita a manipulação de itens e campeões no universo do LoL. O sistema oferece funcionalidades essenciais, como adicionar, buscar, remover itens e campeões, atualizar os atributos dos itens e dos campeões e buscar itens recomendados para um determinado campeão.

## Funcionalidades
- Adicionar, buscar, remover e atualizar itens.
- Adicionar, buscar, remover e atualizar campeões.
- Buscar itens recomendados para um determinado campeão.

## Requisitos do Sistema
- .NET Core SDK
- MySQL
- Dapper
- MySqlConnector
- System.Data.SqlClient
- Swashbuckle.AspNetCore

## Configuração do Banco de Dados
Use as seguintes queries SQL para criar as tabelas necessárias no banco de dados MySQL:

```sql
-- Tabela Classe
CREATE TABLE Classe (
    id INT PRIMARY KEY AUTO_INCREMENT,
    nome VARCHAR(255) NOT NULL,
    UNIQUE (nome)
);

-- Tabela Campeão (atualizada)
CREATE TABLE Campeao (
    id INT PRIMARY KEY AUTO_INCREMENT,
    nome VARCHAR(255) NOT NULL,
    title VARCHAR(255),
    UNIQUE (nome)
);

-- Tabela CampeaoClasse (para relação muitos-para-muitos entre Campeao e Classe)
CREATE TABLE CampeaoClasse (
    campeao_id INT,
    classe_id INT,
    PRIMARY KEY (campeao_id, classe_id),
    FOREIGN KEY (campeao_id) REFERENCES Campeao(id),
    FOREIGN KEY (classe_id) REFERENCES Classe(id)
);

-- Tabela Item
CREATE TABLE Item (
    id INT PRIMARY KEY AUTO_INCREMENT,
    nome VARCHAR(255) NOT NULL,
    custo INT,
    UNIQUE (nome)
);

-- Tabela Tag
CREATE TABLE Tag (
    id INT PRIMARY KEY AUTO_INCREMENT,
    nome VARCHAR(255) NOT NULL,
    UNIQUE (nome)
);

-- Tabela ItemTag (para relação muitos-para-muitos entre Item e Tag)
CREATE TABLE ItemTag (
    item_id INT,
    tag_id INT,
    PRIMARY KEY (item_id, tag_id),
    FOREIGN KEY (item_id) REFERENCES Item(id),
    FOREIGN KEY (tag_id) REFERENCES Tag(id)
);

-- Tabela ItemStats
CREATE TABLE ItemStats (
    id INT PRIMARY KEY AUTO_INCREMENT,
    item_id INT,
    stat_nome VARCHAR(255),
    stat_valor DECIMAL(18, 2),
    FOREIGN KEY (item_id) REFERENCES Item(id)
);
