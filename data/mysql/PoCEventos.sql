DROP SCHEMA IF EXISTS `loja` ;

CREATE SCHEMA IF NOT EXISTS `loja` DEFAULT CHARACTER SET utf8 ;
USE `loja` ;

DROP TABLE IF EXISTS `loja`.`produto` ;

CREATE TABLE IF NOT EXISTS `loja`.`produto` (
  `id` VARCHAR(12) NOT NULL,
  `descricao` VARCHAR(50) NOT NULL,
  `precobrl` DECIMAL(10,2) NOT NULL,
  PRIMARY KEY (`id`))
ENGINE = InnoDB;

DROP TABLE IF EXISTS `loja`.`venda` ;

CREATE TABLE IF NOT EXISTS `loja`.`venda` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `produto_id` VARCHAR(12) NOT NULL,
  `quantidade` INT NOT NULL,
  `precobrl` DECIMAL(10,2) NOT NULL,
  `cotacaousd` DECIMAL(10,4) NOT NULL,
  `taxabrl` DECIMAL(10,2) NOT NULL,
  `totalbrl` DECIMAL(10,2) NOT NULL,
  PRIMARY KEY (`id`),
  CONSTRAINT `fk_venda_produto`
    FOREIGN KEY (`produto_id`)
    REFERENCES `loja`.`produto` (`id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

CREATE INDEX `fk_venda_produto_idx` ON `loja`.`venda` (`produto_id` ASC) VISIBLE;
