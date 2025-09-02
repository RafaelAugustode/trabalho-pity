-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema mydb
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema mydb
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `mydb` DEFAULT CHARACTER SET utf8 ;
USE `mydb` ;

-- -----------------------------------------------------
-- Table `mydb`.`JOGO`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`JOGO` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `custo` DOUBLE NULL,
  `nome` VARCHAR(45) NULL,
  `armazenamento` INT(20) NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`REGRAS`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`REGRAS` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `JOGO_id` INT NOT NULL,
  `explicacao` VARCHAR(45) NULL,
  PRIMARY KEY (`id`, `JOGO_id`),
  INDEX `fk_REGRAS_JOGO1_idx` (`JOGO_id` ASC) VISIBLE,
  CONSTRAINT `fk_REGRAS_JOGO1`
    FOREIGN KEY (`JOGO_id`)
    REFERENCES `mydb`.`JOGO` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`NIVEL`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`NIVEL` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `JOGO_id` INT NOT NULL,
  `ambiente` VARCHAR(45) NULL,
  `dificuldade` VARCHAR(45) NULL,
  `disponibilidade` TINYINT NOT NULL,
  PRIMARY KEY (`id`, `JOGO_id`),
  INDEX `fk_NIVEL_JOGO1_idx` (`JOGO_id` ASC) VISIBLE,
  CONSTRAINT `fk_NIVEL_JOGO1`
    FOREIGN KEY (`JOGO_id`)
    REFERENCES `mydb`.`JOGO` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`DESAFIO`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`DESAFIO` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `categoria` VARCHAR(45) NULL,
  `dificuldade` VARCHAR(45) NULL,
  `completado` TINYINT NULL,
  `NIVEL_id` INT NOT NULL,
  PRIMARY KEY (`id`),
  INDEX `fk_DESAFIO_NIVEL1_idx` (`NIVEL_id` ASC) VISIBLE,
  CONSTRAINT `fk_DESAFIO_NIVEL1`
    FOREIGN KEY (`NIVEL_id`)
    REFERENCES `mydb`.`NIVEL` (`JOGO_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`JOGADOR`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`JOGADOR` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `nome` VARCHAR(45) NULL,
  `senha` VARCHAR(45) NULL,
  `classificacao_etaria` INT(3) NULL,
  `email` VARCHAR(45) NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`AVATAR`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`AVATAR` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `instrumentos` VARCHAR(45) NULL,
  `caracteristica` VARCHAR(45) NULL,
  `fisonomia` VARCHAR(45) NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`ACESSORIO`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`ACESSORIO` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `nome` VARCHAR(45) NULL,
  `melhoria` VARCHAR(45) NULL,
  `caracteristica` VARCHAR(45) NULL,
  `categoria` VARCHAR(45) NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`HABILIDADE`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`HABILIDADE` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `poder` VARCHAR(45) NULL,
  `caracteristica` VARCHAR(45) NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`JOGA`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`JOGA` (
  `JOGADOR_id` INT NOT NULL,
  `JOGO_id` INT NOT NULL,
  `AVATAR_id` INT NOT NULL,
  `progrsso` INT NOT NULL,
  `ultimo_salvamento` VARCHAR(45) NULL,
  `pontuacao` VARCHAR(45) NULL,
  `NIVEL_id` INT NOT NULL,
  `NIVEL_JOGO_id` INT NOT NULL,
  PRIMARY KEY (`JOGADOR_id`, `JOGO_id`, `AVATAR_id`, `NIVEL_id`, `NIVEL_JOGO_id`),
  INDEX `fk_JOGADOR_has_JOGO_JOGO1_idx` (`JOGO_id` ASC) VISIBLE,
  INDEX `fk_JOGADOR_has_JOGO_JOGADOR_idx` (`JOGADOR_id` ASC) VISIBLE,
  INDEX `fk_PARTICIPA_AVATAR1_idx` (`AVATAR_id` ASC) VISIBLE,
  INDEX `fk_JOGA_NIVEL1_idx` (`NIVEL_id` ASC, `NIVEL_JOGO_id` ASC) VISIBLE,
  CONSTRAINT `fk_JOGADOR_has_JOGO_JOGADOR`
    FOREIGN KEY (`JOGADOR_id`)
    REFERENCES `mydb`.`JOGADOR` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_JOGADOR_has_JOGO_JOGO1`
    FOREIGN KEY (`JOGO_id`)
    REFERENCES `mydb`.`JOGO` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_PARTICIPA_AVATAR1`
    FOREIGN KEY (`AVATAR_id`)
    REFERENCES `mydb`.`AVATAR` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_JOGA_NIVEL1`
    FOREIGN KEY (`NIVEL_id` , `NIVEL_JOGO_id`)
    REFERENCES `mydb`.`NIVEL` (`id` , `JOGO_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `mydb`.`AVATAR_ACESSORIO`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `mydb`.`AVATAR_ACESSORIO` (
  `HABILIDADE_id` INT NOT NULL,
  `AVATAR_id` INT NOT NULL,
  `ACESSORIO_id` INT NOT NULL,
  INDEX `fk_AVATAR_ACESSORIO_HABILIDADE1_idx` (`HABILIDADE_id` ASC) VISIBLE,
  INDEX `fk_AVATAR_ACESSORIO_AVATAR1_idx` (`AVATAR_id` ASC) VISIBLE,
  INDEX `fk_AVATAR_ACESSORIO_ACESSORIO1_idx` (`ACESSORIO_id` ASC) VISIBLE,
  CONSTRAINT `fk_AVATAR_ACESSORIO_HABILIDADE1`
    FOREIGN KEY (`HABILIDADE_id`)
    REFERENCES `mydb`.`HABILIDADE` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_AVATAR_ACESSORIO_AVATAR1`
    FOREIGN KEY (`AVATAR_id`)
    REFERENCES `mydb`.`AVATAR` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION,
  CONSTRAINT `fk_AVATAR_ACESSORIO_ACESSORIO1`
    FOREIGN KEY (`ACESSORIO_id`)
    REFERENCES `mydb`.`ACESSORIO` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
