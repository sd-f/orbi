package foundation.softwaredesign.orbi.persistence.entity;

import javax.persistence.*;
import javax.validation.constraints.NotNull;
import java.util.Date;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@Entity
@Table(name = "character", schema = "public")
public class CharacterEntity {

    @Id
    @GeneratedValue(strategy = GenerationType.SEQUENCE, generator = "character_id_gen")
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
    @Column
    @NotNull
    private Double rotationX;
    @Column
    @NotNull
    private Long experiencePoints;
    @Column
    private Long identityId;
    @Column
    @Temporal(TemporalType.TIMESTAMP)
    @NotNull
    private Date lastSeen;
    @Column
    @Temporal(TemporalType.TIMESTAMP)
    private Date giftedOn;
    @OneToOne
    @JoinColumn(name = "identity_id", referencedColumnName = "id", updatable = false, insertable = false)
    private IdentityEntity identityEntity;

    public Long getId() {
        return id;
    }

    public void setId(Long id) {
        this.id = id;
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

    public Double getRotationX() {
        return rotationX;
    }

    public void setRotationX(Double rotationX) {
        this.rotationX = rotationX;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public Long getExperiencePoints() {
        return experiencePoints;
    }

    public void setExperiencePoints(Long experiencePoints) {
        this.experiencePoints = experiencePoints;
    }

    public Long getIdentityId() {
        return identityId;
    }

    public void setIdentityId(Long identityId) {
        this.identityId = identityId;
    }

    public IdentityEntity getIdentityEntity() {
        return identityEntity;
    }

    public void setIdentityEntity(IdentityEntity identityEntity) {
        this.identityEntity = identityEntity;
    }

    public Date getLastSeen() {
        return lastSeen;
    }

    public void setLastSeen(Date lastSeen) {
        this.lastSeen = lastSeen;
    }

    public Date getGiftedOn() {
        return giftedOn;
    }

    public void setGiftedOn(Date giftedOn) {
        this.giftedOn = giftedOn;
    }
}
