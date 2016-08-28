package foundation.softwaredesign.orbi.service;

import com.sun.org.apache.xpath.internal.SourceTree;
import foundation.softwaredesign.orbi.model.auth.AuthorizationInfo;
import foundation.softwaredesign.orbi.service.authorization.TokenThreadLocal;

import javax.annotation.PostConstruct;
import javax.enterprise.context.RequestScoped;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@RequestScoped
public class UserService {

    private AuthorizationInfo authorizationInfo;

    @PostConstruct
    public void init() {
        this.authorizationInfo = new AuthorizationInfo();
        authorizationInfo.setToken(TokenThreadLocal.get());
    }

    public AuthorizationInfo getAuthorizationInfo() {
        return this.authorizationInfo;
    }

}
