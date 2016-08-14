package foundation.softwaredesign.orbi.persistence.entity;

import javax.persistence.*;
import java.math.BigDecimal;
import java.math.BigInteger;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@Entity
@Table(name = "game_object")
public class GameObjectEntity {

    @Id
    @GeneratedValue(strategy = GenerationType.SEQUENCE,generator = "game_object_id_gen")
    private BigInteger id;

    @Column
    String name;

    @Column(precision = 12, scale = 6, nullable = false)
    private BigInteger userId;

    @Column(precision = 12, scale = 6, nullable = false)
    private BigDecimal latitude;

    @Column(precision = 12, scale = 6, nullable = false)
    private BigDecimal longitude;

    @Column(precision = 12, scale = 6, nullable = false)
    private BigDecimal elevation;

    public BigInteger getId() {
        return id;
    }

    public void setId(BigInteger id) {
        this.id = id;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public BigInteger getUserId() {
        return userId;
    }

    public void setUserId(BigInteger userId) {
        this.userId = userId;
    }

    public BigDecimal getLatitude() {
        return latitude;
    }

    public void setLatitude(BigDecimal latitude) {
        this.latitude = latitude;
    }

    public BigDecimal getLongitude() {
        return longitude;
    }

    public void setLongitude(BigDecimal longitude) {
        this.longitude = longitude;
    }

    public BigDecimal getElevation() {
        return elevation;
    }

    public void setElevation(BigDecimal elevation) {
        this.elevation = elevation;
    }
}
