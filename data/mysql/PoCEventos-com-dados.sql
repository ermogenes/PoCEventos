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

START TRANSACTION;

USE `loja`;
INSERT INTO `loja`.`produto` (`id`, `descricao`, `precobrl`) VALUES ('8sad5w8zfgbo', 'Cabo USB tipo C', 7.41);
INSERT INTO `loja`.`produto` (`id`, `descricao`, `precobrl`) VALUES ('gp3ui3y08b6p', 'Fone Xiaomi', 26.44);
INSERT INTO `loja`.`produto` (`id`, `descricao`, `precobrl`) VALUES ('5lc41xhz4sbw', 'Relogio Kaloste', 272.39);
INSERT INTO `loja`.`produto` (`id`, `descricao`, `precobrl`) VALUES ('hcs0cp60csz2', 'Mi Pad 5 Pro', 630.20);
INSERT INTO `loja`.`produto` (`id`, `descricao`, `precobrl`) VALUES ('9y3ak6rh6gx6', 'Lenovo Tab P11', 926.28);
INSERT INTO `loja`.`produto` (`id`, `descricao`, `precobrl`) VALUES ('3052e7j016ir', 'Monitor de Sono Android', 97.51);
INSERT INTO `loja`.`produto` (`id`, `descricao`, `precobrl`) VALUES ('dgdr63hrs6b3', 'Game Stick Lite 4K', 148.62);
INSERT INTO `loja`.`produto` (`id`, `descricao`, `precobrl`) VALUES ('nk0n36069sgg', 'Caneca de Vidro', 24.63);
INSERT INTO `loja`.`produto` (`id`, `descricao`, `precobrl`) VALUES ('8fqupa2mp9o6', 'Boneco Deadpool 8,5 cm', 15.65);
INSERT INTO `loja`.`produto` (`id`, `descricao`, `precobrl`) VALUES ('0cmbtqp9dnmq', 'Faca chaveiro compacta', 25.36);

COMMIT;
