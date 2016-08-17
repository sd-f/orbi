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
    Long longitutde;
    Long latitude;

    public Integer getRid() {
        return rid;
    }

    public void setRid(Integer rid) {
        this.rid = rid;
    }

    public Long getLongitutde() {
        return longitutde;
    }

    public void setLongitutde(Long longitutde) {
        this.longitutde = longitutde;
    }

    public Long getLatitude() {
        return latitude;
    }

    public void setLatitude(Long latitude) {
        this.latitude = latitude;
    }
}
