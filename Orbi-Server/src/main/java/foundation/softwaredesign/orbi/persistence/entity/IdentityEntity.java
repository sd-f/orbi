package foundation.softwaredesign.orbi.persistence.entity;

import foundation.softwaredesign.orbi.model.GameObject;
import foundation.softwaredesign.orbi.persistence.types.ChkPass;
import foundation.softwaredesign.orbi.persistence.types.ChkPassConverter;

import javax.persistence.*;
import javax.validation.constraints.NotNull;
import java.util.Date;
import java.util.List;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@Entity
@Table(name = "identity", schema = "public")
public class IdentityEntity {

    @Id
    @GeneratedValue(strategy = GenerationType.SEQUENCE, generator = "identity_id_gen")
    private Long id;
    @Column
    @NotNull
    private String email;
    @Column
    @Convert(converter = ChkPassConverter.class)
    private ChkPass tmpPassword;
    @Column
    @Convert(converter = ChkPassConverter.class)
    private ChkPass token;
    @Column(precision = 12, scale = 6)
    @NotNull
    private Double latitude;
    @Column(precision = 12, scale = 6)
    @NotNull
    private Double longitude;
    @Column(precision = 12, scale = 6)
    @NotNull
    private Double elevation;
    @Column
    @NotNull
    private Double rotationY;
    @Column
    @NotNull
    private Double rotationX;
    @Column
    @Temporal(TemporalType.TIMESTAMP)
    @NotNull
    private Date lastSeen;

    @OneToMany(mappedBy = "identity", fetch = FetchType.LAZY)
    private List<GameObjectEntity> objects;


    public Long getId() {
        return id;
    }

    public void setId(Long id) {
        this.id = id;
    }

    public ChkPass getToken() {
        return token;
    }

    public void setToken(ChkPass token) {
        this.token = token;
    }

    public String getEmail() {
        return email;
    }

    public void setEmail(String email) {
        this.email = email;
    }

    public ChkPass getTmpPassword() {
        return tmpPassword;
    }

    public void setTmpPassword(ChkPass tmpPassword) {
        this.tmpPassword = tmpPassword;
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

    public Double getElevation() {
        return elevation;
    }

    public void setElevation(Double elevation) {
        this.elevation = elevation;
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

    public Date getLastSeen() {
        return lastSeen;
    }

    public void setLastSeen(Date lastSeen) {
        this.lastSeen = lastSeen;
    }

    public List<GameObjectEntity> getObjects() {
        return objects;
    }

    public void setObjects(List<GameObjectEntity> objects) {
        this.objects = objects;
    }
}
