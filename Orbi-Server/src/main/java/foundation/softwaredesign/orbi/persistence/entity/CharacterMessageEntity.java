package foundation.softwaredesign.orbi.persistence.entity;

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
    @SequenceGenerator(name="character_message_id_seq",
            schema = "public",
            sequenceName="character_message_id_seq", allocationSize=1)
    @GeneratedValue(strategy = GenerationType.SEQUENCE, generator = "character_message_id_seq")
    @Column(updatable = false)
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
