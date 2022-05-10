import org.apache.nifi.serialization.record.RecordField;
import org.apache.nifi.serialization.record.RecordFieldType;

double precoBRL = record.getAsDouble('precobrl')
double quantidade = record.getAsDouble('QUANTIDADE')
double cotacaoUSD = record.getAsDouble('usd')

double ttParcialBRL = precoBRL * quantidade;
double ttUSD = ttParcialBRL * cotacaoUSD
double taxa = ttUSD > 50.0 ? 0.6 : 0

double taxaBRL = (ttParcialBRL * taxa).round(2)
double totalBRL = (ttParcialBRL + ttParcialBRL * taxa).round(2)

record.setValue(new RecordField("cotacaousd", RecordFieldType.DOUBLE.getDataType()), cotacaoUSD)

record.setValue(new RecordField("taxabrl", RecordFieldType.DOUBLE.getDataType()), taxaBRL)

record.setValue(new RecordField("totalbrl", RecordFieldType.DOUBLE.getDataType()), totalBRL)

return record