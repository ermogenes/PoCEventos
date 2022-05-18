import org.apache.nifi.serialization.record.RecordField;
import org.apache.nifi.serialization.record.RecordFieldType;

double precoBRL = record.getAsDouble('precobrl')
int quantidade = record.getAsInt('QUANTIDADE')
double cotacaoUSD = Double.parseDouble(attributes['cotacao'])

double ttParcialBRL = precoBRL * quantidade
double limiteUSD = cotacaoUSD * 50.0
double taxa = ttParcialBRL > limiteUSD ? 0.6 : 0

double taxaBRL = (ttParcialBRL * taxa).round(2)
double totalBRL = (ttParcialBRL + taxaBRL).round(2)

record.setValue(new RecordField("cotacaousd", RecordFieldType.DOUBLE.getDataType()), cotacaoUSD)
record.setValue(new RecordField("taxabrl", RecordFieldType.DOUBLE.getDataType()), taxaBRL)
record.setValue(new RecordField("totalbrl", RecordFieldType.DOUBLE.getDataType()), totalBRL)

return record