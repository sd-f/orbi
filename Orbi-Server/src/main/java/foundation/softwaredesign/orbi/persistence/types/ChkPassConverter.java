package foundation.softwaredesign.orbi.persistence.types;

import org.postgresql.util.PGobject;

import javax.persistence.AttributeConverter;
import javax.persistence.Converter;
import java.sql.SQLException;
import java.util.Objects;
import java.util.logging.Logger;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@Converter
public class ChkPassConverter implements AttributeConverter<ChkPass, PGobject> {


    @Override
    public PGobject convertToDatabaseColumn(ChkPass password) {
        PGobject po = new PGobject();

        po.setType("chkpass");
        try {
            if (Objects.nonNull(password)) {
                po.setValue(password.getValue());
            } else {
                po.setValue(null);
            }

        } catch (SQLException e) {
            Logger.getLogger(ChkPassConverter.class.getName()).severe(e.getMessage());
        }
        return po;
    }

    @Override
    public ChkPass convertToEntityAttribute(PGobject po) {
        if (Objects.nonNull(po)) {
            return new ChkPass(po.getValue());
        }
        return null;
    }
}
