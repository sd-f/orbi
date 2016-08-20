package foundation.softwaredesign.orbi.persistence.entity;

import javax.persistence.*;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@Entity
@Table(name = "game_object")
public class GameObjectEntity {

    @Id
    @GeneratedValue(strategy = GenerationType.SEQUENCE,generator = "game_object_id_gen")
    private Long id;

    @Column
    String name;

    @Column
    private Long userId;

    @Column(precision = 12, scale = 6, nullable = false)
    private Double latitude;

    @Column(precision = 12, scale = 6, nullable = false)
    private Double longitude;

    @Column(precision = 12, scale = 6, nullable = false)
    private Double altitude;

    @Column
    private Double rotationY;

    public Long getId() {
        return id;
    }

    public void setId(Long id) {
        this.id = id;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public Long getUserId() {
        return userId;
    }

    public void setUserId(Long userId) {
        this.userId = userId;
    }

    public Double getLatitude() {
        return latitude;
    }

    public void setLatitude(Double latitude) {
        this.latitude = latitude;
    }

    public Double getLongitude() {
        return longitude;
    }

    public void setLongitude(Double longitude) {
        this.longitude = longitude;
    }

    public Double getAltitude() {
        return altitude;
    }

    public void setAltitude(Double elevation) {
        this.altitude = elevation;
    }

    public Double getRotationY() {
        return rotationY;
    }

    public void setRotationY(Double rotationY) {
        this.rotationY = rotationY;
    }
}
