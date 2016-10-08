package foundation.softwaredesign.orbi.persistence.entity;

import javax.persistence.*;
import javax.validation.constraints.NotNull;
import java.util.Date;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@Entity
@Table(name = "game_object", schema = "public")
public class GameObjectEntity {

    @Id
    @GeneratedValue(strategy = GenerationType.SEQUENCE,generator = "game_object_id_gen")
    private Long id;
    @Column
    @NotNull
    private String name;
    @Column(precision = 12, scale = 6)
    @NotNull
    private Double latitude;
    @Column(precision = 12, scale = 6)
    @NotNull
    private Double longitude;
    @Column(precision = 12, scale = 6)
    @NotNull
    private Double altitude;
    @Column
    @NotNull
    private Double rotationY;
    @Column(insertable = false, updatable = false)
    @Temporal(TemporalType.TIMESTAMP)
    @NotNull
    private Date createDate;

    @Column
    private String userText;

    @ManyToOne
    @NotNull
    private IdentityEntity identity;

    @ManyToOne
    @JoinColumn(name = "game_object_type_id")
    @NotNull
    private GameObjectTypeEntity gameObjectType;

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

    public Date getCreateDate() {
        return createDate;
    }

    public void setCreateDate(Date createDate) {
        this.createDate = createDate;
    }

    public IdentityEntity getIdentity() {
        return identity;
    }

    public void setIdentity(IdentityEntity identity) {
        this.identity = identity;
    }

    public GameObjectTypeEntity getGameObjectType() {
        return gameObjectType;
    }

    public void setGameObjectType(GameObjectTypeEntity gameObjectType) {
        this.gameObjectType = gameObjectType;
    }

    public String getUserText() {
        return userText;
    }

    public void setUserText(String userText) {
        this.userText = userText;
    }
}
