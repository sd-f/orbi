package foundation.softwaredesign.orbi.persistence.entity;

import foundation.softwaredesign.orbi.persistence.types.ChkPass;
import foundation.softwaredesign.orbi.persistence.types.ChkPassConverter;

import javax.persistence.*;
import javax.validation.constraints.NotNull;
import java.util.Date;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@Entity
@Table(name = "character_message", schema = "public")
public class CharacterMessageEntity {

    @Id
    @GeneratedValue(strategy = GenerationType.SEQUENCE, generator = "character_message_id_gen")
    private Long id;
    @Column
    @NotNull
    private Long fromCharacterId;
    @Column
    @NotNull
    private Long toCharacterId;
    @Column
    @NotNull
    private String message;
    @Column
    @Temporal(TemporalType.TIMESTAMP)
    @NotNull
    private Date createdOn;

    public Long getId() {
        return id;
    }

    public void setId(Long id) {
        this.id = id;
    }

    public Long getFromCharacterId() {
        return fromCharacterId;
    }

    public void setFromCharacterId(Long fromCharacterId) {
        this.fromCharacterId = fromCharacterId;
    }

    public Long getToCharacterId() {
        return toCharacterId;
    }

    public void setToCharacterId(Long toCharacterId) {
        this.toCharacterId = toCharacterId;
    }

    public String getMessage() {
        return message;
    }

    public void setMessage(String message) {
        this.message = message;
    }

    public Date getCreatedOn() {
        return createdOn;
    }

    public void setCreatedOn(Date createdOn) {
        this.createdOn = createdOn;
    }
}
