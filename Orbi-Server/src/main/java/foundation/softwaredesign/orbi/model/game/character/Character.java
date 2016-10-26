package foundation.softwaredesign.orbi.model.game.character;

import foundation.softwaredesign.orbi.model.game.transform.Transform;

import javax.xml.bind.annotation.XmlRootElement;
import javax.xml.bind.annotation.XmlTransient;
import java.util.Date;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@XmlRootElement
public class Character {

    private Long id;
    private String name;
    private Long xp = new Long(0);
    private Long xr = new Long(0);
    private Long level = new Long(0);
    private Transform transform = new Transform();
    private CharacterDevelopment characterDevelopment;


    @XmlTransient
    private Date lastSeen;
    @XmlTransient
    private Long identityId;
    @XmlTransient
    private Date giftedOn;

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public Long getXp() {
        return xp;
    }

    public void setXp(Long xp) {
        this.xp = xp;
    }

    public Long getId() {
        return id;
    }

    public void setId(Long id) {
        this.id = id;
    }

    public Transform getTransform() {
        return transform;
    }

    public void setTransform(Transform transform) {
        this.transform = transform;
    }

    public Date getLastSeen() {
        return lastSeen;
    }

    public void setLastSeen(Date lastSeen) {
        this.lastSeen = lastSeen;
    }

    public Long getIdentityId() {
        return identityId;
    }

    public void setIdentityId(Long identityId) {
        this.identityId = identityId;
    }

    public Long getXr() {
        return xr;
    }

    public void setXr(Long xr) {
        this.xr = xr;
    }

    public CharacterDevelopment getCharacterDevelopment() {
        return characterDevelopment;
    }

    public void setCharacterDevelopment(CharacterDevelopment characterDevelopment) {
        this.characterDevelopment = characterDevelopment;
    }

    public Date getGiftedOn() {
        return giftedOn;
    }

    public void setGiftedOn(Date giftedOn) {
        this.giftedOn = giftedOn;
    }

    public Long getLevel() {
        return level;
    }

    public void setLevel(Long level) {
        this.level = level;
    }

}
