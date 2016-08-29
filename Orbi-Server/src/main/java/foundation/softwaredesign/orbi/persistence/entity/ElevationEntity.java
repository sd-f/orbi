package foundation.softwaredesign.orbi.persistence.entity;

import org.eclipse.persistence.annotations.ReadOnly;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;
import javax.persistence.Table;
import javax.validation.constraints.NotNull;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@Entity
@ReadOnly
@Table(name = "elevation", schema = "public")
public class ElevationEntity {

    @Id
    private Integer rid;
    @Column
    @NotNull
    private Long latitude;
    @Column
    @NotNull
    private Long longitude;

    public Integer getRid() {
        return rid;
    }

    public void setRid(Integer rid) {
        this.rid = rid;
    }

    public Long getLongitude() {
        return longitude;
    }

    public void setLongitude(Long longitutde) {
        this.longitude = longitutde;
    }

    public Long getLatitude() {
        return latitude;
    }

    public void setLatitude(Long latitude) {
        this.latitude = latitude;
    }
}
