package foundation.softwaredesign.orbi.persistence.entity;

import javax.persistence.*;
import java.math.BigDecimal;
import java.math.BigInteger;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@Entity
@Table(name = "elevation")
public class ElevationEntity {

    @Id
    @GeneratedValue(strategy = GenerationType.SEQUENCE,generator = "elevation_id_gen")
    Integer rid;
    BigInteger longitutde;
    BigInteger latitude;

    public Integer getRid() {
        return rid;
    }

    public void setRid(Integer rid) {
        this.rid = rid;
    }

    public BigInteger getLongitutde() {
        return longitutde;
    }

    public void setLongitutde(BigInteger longitutde) {
        this.longitutde = longitutde;
    }

    public BigInteger getLatitude() {
        return latitude;
    }

    public void setLatitude(BigInteger latitude) {
        this.latitude = latitude;
    }
}
