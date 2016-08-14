package foundation.softwaredesign.orbi.persistence.entity;

import org.eclipse.persistence.annotations.ReadOnly;

import javax.persistence.Entity;
import javax.persistence.Id;
import javax.persistence.Table;
import java.math.BigInteger;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@Entity
@ReadOnly
@Table(name = "elevation")
public class ElevationEntity {

    @Id
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
