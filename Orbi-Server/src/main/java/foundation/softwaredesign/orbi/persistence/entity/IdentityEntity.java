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
    @Column
    @Temporal(TemporalType.TIMESTAMP)
    @NotNull
    private Date lastInit;

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

    public Date getLastInit() {
        return lastInit;
    }

    public void setLastInit(Date lastSeen) {
        this.lastInit = lastSeen;
    }

}
